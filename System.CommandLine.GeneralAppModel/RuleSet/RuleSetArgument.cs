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
            return base.Report(tabsCount) +
                   $@"{CoreExtensions.NewLineWithTabs(tabsCount)} Required Rules: { string.Join("", RequiredRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription))}
                      {CoreExtensions.NewLineWithTabs(tabsCount)} Arity Rules:  { string.Join("", ArityRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription))}
                      {CoreExtensions.NewLineWithTabs(tabsCount)} SpecialArgumentType Rule:  { string.Join("", RequiredRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription))}";
        }
    }
}