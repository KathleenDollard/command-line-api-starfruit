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
            return base.Report(tabsCount)
                    + RequiredRules.ReportRuleGroup(tabsCount, "whether a value is required");      
        }

    }
}

