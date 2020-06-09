﻿namespace System.CommandLine.GeneralAppModel
{
    public class Strategy
    {
        public static Strategy Full = new Strategy("Full").SetFullRules();
        public static Strategy Standard = new Strategy("Standard").SetStandardRules();

        public Strategy(string name="")
        {
            Name = name;
        }
        public string Name { get; }
        public RuleSetGetCandidatesRule GetCandidateRules { get; } = new RuleSetGetCandidatesRule();
        public RuleSetSelectSymbols SelectSymbolRules { get; } = new RuleSetSelectSymbols();
        public RuleSetArgument ArgumentRules { get; } = new RuleSetArgument();
        public RuleSetOption OptionRules { get; } = new RuleSetOption();
        public RuleSetArgument OptionArgumentRules { get; } = new RuleSetArgument(); // these aren't yet used
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
