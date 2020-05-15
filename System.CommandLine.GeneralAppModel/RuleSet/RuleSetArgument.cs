using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetArgument : RuleSetSymbol, IRuleSetArgument
    {
        public RuleSetArgument OptionArgumentRuleSet { get; }
        public RuleGroup<IRuleArity> ArityRules { get; } = new RuleGroup<IRuleArity>();
        public RuleGroup<IRuleGetValues<bool>> RequiredRules { get; } = new RuleGroup<IRuleGetValues<bool>>();
        public RuleGroup<IRuleGetValues<Type>> SpecialArgumentTypeRules { get; } = new RuleGroup<IRuleGetValues<Type>>();

        public void AddArityRule(IRuleArity arityRule)
        {
            CheckFrozen();
            ArityRules.Add(arityRule);
        }

        public void AddRequiredRule(IRuleGetValues<bool> requiredRule)
        {
            CheckFrozen();
            RequiredRules.Add(requiredRule);
        }

        public void AddSpecialArgumentTypeRule(IRuleGetValues<Type> specialArgumentType)
        {
            CheckFrozen();
            SpecialArgumentTypeRules.Add(specialArgumentType);
        }

        public override string Report(int tabsCount)
        {
            return RequiredRules.ReportRuleGroup(tabsCount, "whether a value is required")
                    + ArityRules.ReportRuleGroup(tabsCount, "the arity (number of values allowed)")
                    + SpecialArgumentTypeRules.ReportRuleGroup(tabsCount, "argument type in a special way");

            //return base.Report(tabsCount) +
            //       $@"{CoreExtensions.NewLineWithTabs(tabsCount)}To determine whether the option is required: { string.Join("", RequiredRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription + $" ({r.GetType().Name})"))}
            //          {CoreExtensions.NewLineWithTabs(tabsCount)}To determine the arity (number of values allowed):  { string.Join("", ArityRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription + $" ({r.GetType().Name})"))}
            //          {CoreExtensions.NewLineWithTabs(tabsCount)}Special way to determine the argument type:  { string.Join("", SpecialArgumentTypeRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription + $" ({r.GetType().Name})"))}";
        }
    }
}