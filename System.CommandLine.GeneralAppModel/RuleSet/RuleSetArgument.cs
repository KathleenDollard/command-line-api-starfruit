using System.CommandLine.GeneralAppModel.Rules;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetArgument : RuleSetSymbol, IRuleSetArgument
    {
        public RuleSetArgument OptionArgumentRuleSet { get; }
        public RuleGroup<IRuleArity> ArityRules { get; } = new RuleGroup<IRuleArity>();
        public RuleGroup<IRuleOptionalValue<object>> DefaultRules { get; } = new RuleGroup<IRuleOptionalValue<object>>();
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
            return base.Report(tabsCount)
                    + RequiredRules.ReportRuleGroup(tabsCount, "whether a value is required")
                    + ArityRules.ReportRuleGroup(tabsCount, "the arity (number of values allowed)")
                    + SpecialArgumentTypeRules.ReportRuleGroup(tabsCount, "argument type in a special way");

        }
    }
}