namespace System.CommandLine.GeneralAppModel
{
    public class Strategy
    {
        public Strategy(string name=null)
        {
            Name = name;
        }
        public string Name { get; }
        public RuleSetGetCandidatesRule GetCandidateRules { get; } = new RuleSetGetCandidatesRule();
        public RuleSetSelectSymbols SelectSymbolRules { get; } = new RuleSetSelectSymbols();
        public RuleSetArgument ArgumentRules { get; } = new RuleSetArgument();
        public RuleSetOption OptionRules { get; } = new RuleSetOption();
        public RuleSetCommand CommandRules { get; } = new RuleSetCommand();

        public string Report()
        {
            return $@"
Strategy: {Name}
   To classify symols:{ SelectSymbolRules.Report(2)}
   For argument details:{ ArgumentRules.Report(2)}
   For option details:{ OptionRules.Report(2)}
   For command details:{ CommandRules.Report(2)}
";
        }

    }
}
