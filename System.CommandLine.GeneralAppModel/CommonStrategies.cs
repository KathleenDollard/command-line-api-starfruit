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
    }
}
