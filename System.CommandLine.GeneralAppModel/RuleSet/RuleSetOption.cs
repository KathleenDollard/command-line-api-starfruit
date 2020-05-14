using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetOption : RuleSetSymbol, IRuleSetOption
    {
        public RuleGroup<IRuleGetValues<bool>> RequiredRules { get; } = new RuleGroup<IRuleGetValues<bool>>();

        public void AddRequiredRule(IRuleGetValues<bool> requiredRule)
        {
            CheckFrozen();
            RequiredRules.Add(requiredRule);
        }

        public override string Report(int tabsCount)
        {
            return base.Report(tabsCount) +
                     $@"{CoreExtensions.NewLineWithTabs(tabsCount)}Required Rules: { string.Join("", RequiredRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription))}";
        }

    }
}

