namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetOption : RuleSetSymbols, IRuleSetOption
    {
        public RuleSet<IRuleGetValues<bool>> RequiredRule { get; } = new RuleSet<IRuleGetValues<bool>>();

        public void AddRequiredRule(IRuleGetValues<bool> requiredRule)
        {
            CheckFrozen();
            RequiredRule.Add(requiredRule);
        }


    }
}