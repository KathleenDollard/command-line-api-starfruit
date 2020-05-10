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

        protected abstract IEnumerable<T> GetMatchingItems(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items);
        public abstract bool HasMatch(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items);

        public virtual IEnumerable<T> GetAllNonDefault(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
              => GetMatchingItems(symbolDescriptor, items)
                       .Where(v => !v.Equals(default));

        public RuleBase<T> WithSymbolType(SymbolType symbolType)
        {
            var ret = (RuleBase<T>)MemberwiseClone();
            ret.SymbolType = symbolType;
            return ret;
        }

        public virtual (bool Success, T Value) GetSingleOrDefault(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            var matches = GetMatchingItems(symbolDescriptor, items)
                                .Where(v => !v.Equals(default));
            return matches.Any()
                       ? (true, matches.FirstOrDefault())
                       : (false, default);
        }
    }
}
