namespace System.CommandLine.GeneralAppModel
{
    public static class StandardStrategy
    {
        public static Strategy SetStandardRules(this Strategy strategy)
        {
            StandardRules.SetDescriptorContextRules(strategy.DescriptorContextRules);
            StandardRules.SetCandidatesRules(strategy.GetCandidateRules);
            StandardRules.SetSelectSymbolRules(strategy.SelectSymbolRules);
            StandardRules.SetArgumentRules(strategy.ArgumentRules);
            StandardRules.SetCommandRules(strategy.CommandRules);
            StandardRules.SetOptionRules(strategy.OptionRules);
            StandardRules.SetOptionArgumentRules(strategy.OptionArgumentRules);
            return strategy;
        }
    }
}
