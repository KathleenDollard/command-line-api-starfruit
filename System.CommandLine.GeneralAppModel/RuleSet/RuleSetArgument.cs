namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetArgument : RuleSetSymbol
    {
        public RuleGroup<IRuleArity> ArityRules { get; } = new RuleGroup<IRuleArity>();
        public RuleGroup<IRuleOptionalValue<object>> DefaultValueRules { get; } = new RuleGroup<IRuleOptionalValue<object>>();
        public RuleGroup<IRuleGetValue<bool>> RequiredRules { get; } = new RuleGroup<IRuleGetValue<bool>>();
        public RuleGroup<IRuleGetValue<Type>> SpecialArgumentTypeRules { get; } = new RuleGroup<IRuleGetValue<Type>>();

        //public void AddArityRule(IRuleArity arityRule)
        //{
        //    ArityRules.Add(arityRule);
        //}

        //public void AddRequiredRule(IRuleGetValues<bool> requiredRule)
        //{
        //    RequiredRules.Add(requiredRule);
        //}

        //public void AddSpecialArgumentTypeRule(IRuleGetValues<Type> specialArgumentType)
        //{
        //    SpecialArgumentTypeRules.Add(specialArgumentType);
        //}

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