using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Text;

namespace System.CommandLine.ReflectionAppModel
{
    public static class StandardRules
    {
        public static Strategy SetStandardAvailableCandidatesRules(this Strategy strategy)
        {
            strategy.GetCandidateRules.AvailableCandidatesRules();
            return strategy;
        }

        public static RuleSetGetCandidatesRule AvailableCandidatesRules(this RuleSetGetCandidatesRule rules)
        {
            rules.Rules
                .Add(new DerivedTypeRule())
                ;
            return rules;
        }
    }
}
