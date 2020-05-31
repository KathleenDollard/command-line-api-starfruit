namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetCommand : RuleSetSymbol
    {
        public RuleGroup<IRuleGetValue<bool>> TreatUnmatchedTokensAsErrorsRules { get; } = new RuleGroup<IRuleGetValue<bool>>();
        public void TreatUnmatchedTokensAsErrors(IRuleGetValue<bool> rule)
        {
            TreatUnmatchedTokensAsErrorsRules.Add(rule);
        }
    }
}