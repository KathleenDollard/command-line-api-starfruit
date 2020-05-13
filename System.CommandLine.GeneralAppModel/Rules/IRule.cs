namespace System.CommandLine.GeneralAppModel
{
    public interface IRule
    {
        string RuleDescription { get; }
    }

    public interface IRule<T> : IRule
    { }

}
