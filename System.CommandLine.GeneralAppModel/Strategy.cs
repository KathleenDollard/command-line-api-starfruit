namespace System.CommandLine.GeneralAppModel
{
    public class Strategy
    {
        public Strategy(string name=null)
        {
            Name = name;
        }
        public string Name { get; }
        public RuleSetSelectSymbols SelectSymbolRules { get; } = new RuleSetSelectSymbols();
        public RuleSetArgument ArgumentRules { get; } = new RuleSetArgument();
        public RuleSetOption OptionRules { get; } = new RuleSetOption();
        public RuleSetCommand CommandRules { get; } = new RuleSetCommand();

        public string Report()
        {
            return $@"
Strategy: {Name}
   SelectSymbolRules:{ SelectSymbolRules.Report(1)}
   ArgumentRules:{ ArgumentRules.Report(1)}
   OptionRules:{ OptionRules.Report(1)}
   CommandRules:{ CommandRules.Report(1)}
";
        }

    }
}
