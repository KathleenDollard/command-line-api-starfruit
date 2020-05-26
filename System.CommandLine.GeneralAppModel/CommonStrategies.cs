namespace System.CommandLine.GeneralAppModel
{
    public static class CommonStrategies
    {
        public static Strategy SetGeneralRules(this Strategy strategy) 
            => strategy
                .SetFullSelectSymbolRules()
                .SetFullArgumentRules()
                .SetFullCommandRules()
                .SetFullOptionRules();

        public static Strategy CandidateNamesToIgnore(this Strategy strategy, params string[] namesToIgnore)
        {
            strategy.GetCandidateRules.NamesToIgnore.AddRange(namesToIgnore);
            return strategy;
        }
    }
}
