namespace System.CommandLine.GeneralAppModel
{
    public static class CommonStrategies
    {
        public static Strategy SetGeneralRules(this Strategy strategy) 
            => strategy
                .SetStandardSelectSymbolRules()
                .SetStandardArgumentRules()
                .SetStandardCommandRules()
                .SetStandardOptionRules();

        public static Strategy CandidateNamesToIgnore(this Strategy strategy, params string[] namesToIgnore)
        {
            strategy.GetCandidateRules.NamesToIgnore.AddRange(namesToIgnore);
            return strategy;
        }
    }
}
