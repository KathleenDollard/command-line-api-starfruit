using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class RuleBase
    {
        public RuleBase(SymbolType symbolType)
        {
            SymbolType = symbolType;
        }

   
        public SymbolType SymbolType { get; private protected  set; }
        public abstract string RuleDescription { get; }
    }

    public abstract class RuleBase<T> : RuleBase
    {
        public RuleBase(SymbolType symbolType) : base(symbolType)
        { }

        protected abstract IEnumerable<T> GetMatchingItems(SymbolType symbolType, object[] items);
        public abstract bool HasMatch(SymbolType symbolType, object[] items);

        public virtual IEnumerable<T> GetAllNonDefault(SymbolType symbolType, object[] items)
              => GetMatchingItems(symbolType, items)
                       .Where(v => !v.Equals(default));

        public RuleBase<T> WithSymbolType(SymbolType symbolType)
        {
            var ret = (RuleBase<T>)MemberwiseClone();
            ret.SymbolType = symbolType;
            return ret;
        }

        public virtual (bool Success, T Value) GetSingleOrDefault(SymbolType symbolType, object[] items)
        {
            var matches = GetMatchingItems(symbolType, items)
                                .Where(v => !v.Equals(default));
            return matches.Any()
                       ? (true, matches.FirstOrDefault())
                       : (false, default);
        }
    }
}
