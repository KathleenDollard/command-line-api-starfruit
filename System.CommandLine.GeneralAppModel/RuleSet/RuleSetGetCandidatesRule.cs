using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetGetCandidatesRule : RuleSetBase
    {
        public RuleGroup<IRuleGetAvailableCandidates> Rules { get; } = new RuleGroup<IRuleGetAvailableCandidates>();


        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> candidates, SymbolDescriptorBase parentSymbolDescriptor)
        {
            return Rules
                     .OfType<IRuleGetAvailableCandidates>()
                     .SelectMany(r => r.GetAvailableCandidates( parentSymbolDescriptor))
                     .ToList();
        }

     public override string Report(int tabsCount)
        {
            return   $@"{CoreExtensions.NewLineWithTabs(tabsCount)}Arity Rules:  { string.Join("", Rules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription))}";
        }
    }
}