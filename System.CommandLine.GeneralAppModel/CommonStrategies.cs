namespace System.CommandLine.GeneralAppModel
{
    public static class CommonStrategies
    {
        public static Strategy SetStandardRules(this Strategy strategy) 
            => strategy
                .SetStandardAvailableCandidatesRules()
                .SetStandardSelectSymbolRules()
                .SetStandardArgumentRules()
                .SetStandardCommandRules()
                .SetStandardOptionRules();
    }
}
