using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// In order to support remaining items (those not in the other two groups) being identifiable
    /// order matters, and the last can have a RemainingItemRule
    /// </remarks>
    public class RuleSetSelectSymbols : RuleSetBase
    {
        public RuleGroup<IRuleGetCandidates> Rules { get; private set; } = new RuleGroup<IRuleGetCandidates>();


        public IEnumerable<Candidate> GetItems(SymbolType symbolType,
                                               SymbolDescriptorBase commandDescriptor,
                                               IEnumerable<Candidate> candidates)
        {
            IEnumerable<IRuleGetCandidates> rules = Rules
                                .OfType<IRuleGetCandidates>()
                                .Where(x => x.SymbolType == symbolType)
                                .ToList();
            return rules                 
                    .SelectMany(r => r.GetCandidates(candidates, commandDescriptor))
                    .Distinct(new CompareRaw())
                    .ToList();
        }

        private class CompareRaw : IEqualityComparer<Candidate>
        {
            public bool Equals(Candidate x, Candidate y) 
                => x.Item == y.Item;

            public int GetHashCode(Candidate obj) 
                => obj.Item.GetHashCode();
        }

        public override string Report(int tabsCount)
        {
            string whitespace = CoreExtensions.NewLineWithTabs(tabsCount);
            return string.Join("", Rules.Select(r => whitespace + $"Is {r.SymbolType.ToString().ProperAnOrA()} if {r.RuleDescription<IRuleGetCandidates>()} ({r.GetType().Name})"));
        }
    }
}
