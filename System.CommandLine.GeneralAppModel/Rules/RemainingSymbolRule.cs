﻿using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public class RemainingSymbolRule : RuleBase, IRuleGetCandidates
    {
        public RemainingSymbolRule(SymbolType symbolType)
            : base(symbolType)
        {
        }



        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> items, ISymbolDescriptor parentSymbolDescriptor)
        {
            return items;
        }

        public override string RuleDescription<TIRuleSet>()
            => "that is not already matched";
    }
}
