namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetArgument : RuleSetSymbol
    {
        public RuleGroup<IRuleArity> ArityRules { get; } = new RuleGroup<IRuleArity>();
        public RuleGroup<IRuleOptionalValue<object>> DefaultValueRules { get; } = new RuleGroup<IRuleOptionalValue<object>>();
        public RuleGroup<IRuleGetValue<bool>> RequiredRules { get; } = new RuleGroup<IRuleGetValue<bool>>();
        public RuleGroup<IRuleGetValues<object[]>> AllowedValueRules { get; } = new RuleGroup<IRuleGetValues<object[]>>();
        public RuleGroup<IRuleGetValue<Type>> SpecialArgumentTypeRules { get; } = new RuleGroup<IRuleGetValue<Type>>();

        public override void ReplaceAbstractRules(SpecificSource tools)
        {
            base.ReplaceAbstractRules(tools);
            ArityRules.ReplaceAbstractRules(tools);
            DefaultValueRules.ReplaceAbstractRules(tools);
            RequiredRules.ReplaceAbstractRules(tools);
            SpecialArgumentTypeRules.ReplaceAbstractRules(tools);
        }

        public override string Report(int tabsCount)
        {
            return base.Report(tabsCount)
                    + RequiredRules.ReportRuleGroup(tabsCount, "whether a value is required")
                    + ArityRules.ReportRuleGroup(tabsCount, "the arity (number of values allowed)")
                    + SpecialArgumentTypeRules.ReportRuleGroup(tabsCount, "argument type in a special way");

        }
    }
}