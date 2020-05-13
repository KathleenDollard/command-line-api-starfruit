namespace System.CommandLine.GeneralAppModel
{
    public static class CommonStrategies
    {
        public static Strategy SetStandardRules(this Strategy strategy) 
            => strategy
                .SetStandardSelectSymbolRules()
                .SetStandardArgumentRules()
                .SetStandardCommandRules()
                .SetStandardOptionRules();
    }
}
