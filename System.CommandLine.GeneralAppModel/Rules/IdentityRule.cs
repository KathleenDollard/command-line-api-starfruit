using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public class IdentityRule<T> : RuleBase<T>
    {
        public IdentityRule(SymbolType symbolType = SymbolType.All)
              : base(symbolType)
        { }

        public override string RuleDescription
            => "Identity Rule";

        public override bool HasMatch(SymbolType symbolType ,object[] items)
        {
            return items
                    .Any();

        }

        protected override IEnumerable<T> GetMatchingItems(SymbolType symbolType, object[] items)
        {
            return SymbolType != SymbolType.All && SymbolType != symbolType
                ? Array.Empty<T>()
                : items
                    .OfType<IdentityWrapper<T>>()
                    .Select(w => w.Value);
        }
    }

    public class IdentityWrapper
    { }

    public class IdentityWrapper<T> : IdentityWrapper 
    {
        public IdentityWrapper(T value)
        {
            Value = value;
        }
        public T Value { get;  }
    }
}
