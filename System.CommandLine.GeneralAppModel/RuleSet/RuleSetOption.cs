namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetOption : RuleSetSymbol
    {
        public RuleGroup<IRuleGetValue<bool>> RequiredRules { get; } = new RuleGroup<IRuleGetValue<bool>>();

        public void AddRequiredRule(IRuleGetValue<bool> requiredRule)
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

