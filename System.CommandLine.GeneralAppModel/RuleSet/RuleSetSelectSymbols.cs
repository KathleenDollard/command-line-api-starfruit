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
                                               ISymbolDescriptor commandDescriptor,
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

        public override void ReplaceAbstractRules(DescriptorMakerSpecificSourceBase tools)
        {
            Rules.ReplaceAbstractRules(tools);
        }

        public override string Report(int tabsCount)
        {
            string whitespace = CoreExtensions.NewLineWithTabs(tabsCount);
            return string.Join("", Rules.Select(r => whitespace + 
                                    $"{ProperName(r.SymbolType)} if " +
                                    $"{r.RuleDescription<IRuleGetCandidates>()} " +
                                    $"({r.GetType().NameWithGenericArguments()})"));

            string ProperName(SymbolType symbolType)
            {
                return symbolType.ToString();
            }
        }

        public IEnumerable<ReportStructure> GetRulesReportStructure ()
        {
            return Rules.Select(x=>new ReportStructure(x.SymbolType.ToString(), x.RuleDescription<IRuleGetCandidates>(),x.GetType()));
        }

        public struct ReportStructure
        {
            public ReportStructure(string symbolName, string description, Type ruleType)
            {
                SymbolName = symbolName;
                Description = description;
                RuleType = ruleType;
            }

            public string SymbolName { get; }
            public string Description { get; }
            public Type RuleType { get; }
        }
    }
}
