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
        public List<string> NamesToIgnore { get; } = new List<string>();
        public RuleGroup<IRuleGetCandidates> Rules { get; private set; } = new RuleGroup<IRuleGetCandidates>();


        public IEnumerable<Candidate> GetItems(SymbolType symbolType,
                                               SymbolDescriptorBase commandDescriptor,
                                               IEnumerable<Candidate> candidates)
        {
            return Rules
                    .OfType<IRuleGetCandidates>()
                    .Where(x => x.SymbolType == symbolType)
                    .SelectMany(r => r.GetCandidates(candidates, commandDescriptor))
                    .ToList();
        }

        public override string Report(int tabsCount)
        {
            string whitespace = CoreExtensions.NewLineWithTabs(tabsCount);
            return string.Join("", Rules.Select(r => whitespace + $"Is {r.SymbolType.ToString().ProperAnOrA()} if {r.RuleDescription<IRuleGetCandidates>()} ({r.GetType().Name})"));
        }
    }
}
