namespace System.CommandLine.GeneralAppModel
{
    public class RuleUseOption : RuleUseSymbols, IRuleUseOption
    {
        public IRuleGetValues<bool> RequiredRule { get; }
    }
}