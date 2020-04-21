using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public class LabelRule<T> : RuleBase<T>
    {
        public LabelRule(string label, SymbolType symbolType = SymbolType.All)
            : base(symbolType)
        {
            Label = label;
        }

        public string Label { get; }

        public override string RuleDescription { get; }

        public override bool HasMatch(SymbolType symbolType, object[] items)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<T> GetMatchingItems(SymbolType symbolType, object[] items)
        {
            throw new NotImplementedException();
        }
    }
}
