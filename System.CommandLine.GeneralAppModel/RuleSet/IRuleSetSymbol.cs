namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleSetSymbol 
    {
        RuleGroup<IRuleGetValue<string>> DescriptionRules { get; }
        RuleGroup<IRuleGetValue<string>> NameRules { get; }
        RuleGroup<IRuleAliases> AliasRules { get; }
        RuleGroup<IRuleGetValue<bool>> IsHiddenRules { get; }
    }

}
