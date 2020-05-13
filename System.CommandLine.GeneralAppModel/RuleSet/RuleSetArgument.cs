using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetArgument : RuleSetSymbols, IRuleSetArgument
    {
        public RuleSetArgument OptionArgumentRuleSet { get; } 
        public RuleSet<IRuleArity> ArityRule { get; } = new RuleSet<IRuleArity>();
        public RuleSet<IRuleGetValues<bool>> RequiredRule { get; } = new RuleSet<IRuleGetValues<bool>>();
        public RuleSet<IRuleGetValues<Type>> SpecialArgumentTypeRule { get; } = new RuleSet<IRuleGetValues<Type>>();

        public void AddArityRule(IRuleArity arityRule)
        {
            CheckFrozen();
            ArityRule.Add( arityRule);
        }

        public void AddRequiredRule(IRuleGetValues<bool> requiredRule)
        {
            CheckFrozen();
            RequiredRule.Add(requiredRule);
        }

        public void AddSpecialArgumentTypeRule(IRuleGetValues<Type> specialArgumentType)
        {
            CheckFrozen();
            SpecialArgumentTypeRule.Add(specialArgumentType);
        }
    }
}