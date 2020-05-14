using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Rules
{
    public class RemainingSymbolRule : RuleBase, IRuleGetCandidates
    {
        public RemainingSymbolRule(SymbolType symbolType)
            : base(symbolType)
        {
        }

        public override string RuleDescription
            => "RemainingSymbolRule: All not yet matched";

        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> items, SymbolDescriptorBase parentSymbolDescriptor)
        {
            return items;
        }

    }
}
