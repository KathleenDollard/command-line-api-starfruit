﻿using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{

    public class RuleSetGetCandidatesRule : RuleSetBase
    {
        public RuleGroup<IRuleGetAvailableCandidates> Rules { get; } = new RuleGroup<IRuleGetAvailableCandidates>();

        public List<string> NamesToIgnore { get; } = new List<string>();

        public IEnumerable<Candidate> GetCandidates(SymbolDescriptor commandDescriptor)
        {
            return Rules
                     .OfType<IRuleGetAvailableCandidates>()
                     .SelectMany(r => r.GetChildCandidates(commandDescriptor))
                     .ToList();
        }

        public override void ReplaceAbstractRules(DescriptorMakerSpecificSourceBase tools)
        {
            Rules.ReplaceAbstractRules(tools);
        }

        public override string Report(int tabsCount)
        {
            return $@"{CoreExtensions.NewLineWithTabs(tabsCount)}Arity Rules:  { string.Join("", Rules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription<RuleSetGetCandidatesRule>()))}";
        }

        public IEnumerable<ReportStructure> GetRulesReportStructure()
        {
            return Rules.Select(x => new ReportStructure(x.SymbolType.ToString(), x.RuleDescription<IRuleGetCandidates>(), x.GetType()));
        }

    }
}