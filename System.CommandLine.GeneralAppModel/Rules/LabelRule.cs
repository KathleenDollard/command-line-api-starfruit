using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public class LabelRule : RuleBase
    {
        public LabelRule(string label, SymbolType symbolType = SymbolType.All)
            : base(symbolType)
        {
            Label = label;
        }

        public string Label { get; }

        public override string RuleDescription { get; }

        public override bool HasMatch(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> GetMatchingItems(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            throw new NotImplementedException();
        }
    }
}
