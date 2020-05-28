namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleSetOption : IRuleSetSymbol
    {
        RuleGroup<IRuleGetValue<bool>> RequiredRules { get; }
        // TODO: We need a rule to recognize arguments
    }

}
