namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleSetSymbol 
    {
        RuleGroup<IRuleGetValues<string>> DescriptionRules { get; }
        RuleGroup<IRuleGetValues<string>> NameRules { get; }
        RuleGroup<IRuleAliases> AliasesRules { get; }
        RuleGroup<IRuleGetValues<bool>> IsHiddenRules { get; }
    }

}
