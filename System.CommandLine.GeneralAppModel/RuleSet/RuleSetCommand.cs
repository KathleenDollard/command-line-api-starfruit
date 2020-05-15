namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetCommand : RuleSetSymbol, IRuleSetCommand
    {
        public RuleGroup<IRuleGetValues<bool>> TreatUnmatchedTokensAsErrorsRules { get; } = new RuleGroup<IRuleGetValues<bool>>();
        public void TreatUnmatchedTokensAsErrors(IRuleGetValues<bool> rule)
        {
            CheckFrozen();
            TreatUnmatchedTokensAsErrorsRules.Add(rule);
        }
    }
}