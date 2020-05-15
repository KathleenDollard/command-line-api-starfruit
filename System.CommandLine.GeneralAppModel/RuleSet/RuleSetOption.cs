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
            return RequiredRules.ReportRuleGroup(tabsCount, "whether a value is required");      
            //return base.Report(tabsCount) +
            //         $@"{CoreExtensions.NewLineWithTabs(tabsCount)}To determine whether the option is required: { string.Join("", RequiredRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription + $" ({r.GetType().Name})"))}";
        }

    }
}

