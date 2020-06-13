namespace System.CommandLine.GeneralAppModel
{
    public static class StandardStrategy
    {
 

        public static Strategy SetStandardRules(this Strategy strategy)
        {
            StandardRules.SetSelectSymbolRules(strategy.SelectSymbolRules);
            StandardRules.SetCandidatesRules(strategy.GetCandidateRules);
            StandardRules.SetArgumentRules(strategy.ArgumentRules);
            StandardRules.SetCommandRules(strategy.CommandRules);
            StandardRules.SetOptionRules(strategy.OptionRules);
            StandardRules.SetOptionArgumentRules(strategy.OptionArgumentRules);
            return strategy;
        }
    }
}
