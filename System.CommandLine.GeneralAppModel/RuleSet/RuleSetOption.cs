namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetOption : RuleSetSymbols, IRuleSetOption
    {
        public RuleSet<IRuleGetValues<bool>> RequiredRule { get; private set; }

        public void AddRequiredRule(IRuleGetValues<bool> requiredRule)
        {
            CheckFrozen();
            RequiredRule.Add(requiredRule);
        }


    }
}