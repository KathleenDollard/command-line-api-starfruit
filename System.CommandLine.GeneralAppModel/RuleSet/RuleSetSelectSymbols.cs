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
        public RuleGroup<IRuleGetItems> Rules { get; private set; } = new RuleGroup<IRuleGetItems>();


        public IEnumerable<Candidate> GetItems(SymbolType symbolType,
                                               SymbolDescriptorBase commandDescriptor,
                                               IEnumerable<Candidate> candidates)
        {
            return Rules
                    .OfType<IRuleGetItems>()
                    .Where(x => x.SymbolType == symbolType)
                    .SelectMany(r => r.GetItems(candidates, commandDescriptor))
                    .ToList();
        }

        public override string Report(int tabsCount)
        {
            string whitespace = CoreExtensions.NewLineWithTabs(tabsCount);
            return string.Join("", Rules.Select(r => whitespace + r.RuleDescription));
        }
    }
}
