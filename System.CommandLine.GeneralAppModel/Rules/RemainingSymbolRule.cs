using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Rules
{
    public class RemainingSymbolRule : RuleBase, IRuleGetItems
    {
        public RemainingSymbolRule(SymbolType symbolType)
            : base(symbolType)
        {
        }

        public override string RuleDescription
            => "RemainingSymbolRule: All not yet matched";

        public IEnumerable<Candidate> GetItems(IEnumerable<Candidate> items, SymbolDescriptorBase parentSymbolDescriptor)
        {
            return items;
        }

    }
}
