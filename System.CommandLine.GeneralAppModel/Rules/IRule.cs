namespace System.CommandLine.GeneralAppModel
{
    public interface IRule
    {
        string RuleDescription<TIRuleSet>();
 
        SymbolType SymbolType { get; }
    }

    public interface IRule<T> : IRule
    { }

}
