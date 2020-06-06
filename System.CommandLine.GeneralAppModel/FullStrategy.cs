namespace System.CommandLine.GeneralAppModel
{
    public static class FullStrategy
    {
        public static Strategy SetFullRules(this Strategy strategy)
        {
            FullRules.SetFullSelectSymbolRules(strategy.SelectSymbolRules);
            FullRules.SetFullCandidatesRules(strategy.GetCandidateRules);
            FullRules.SetFullArgumentRules(strategy.ArgumentRules);
            FullRules.SetFullCommandRules(strategy.CommandRules);
            FullRules.SetFullOptionRules(strategy.OptionRules);
            FullRules.SetFullOptionArgumentRules(strategy.OptionArgumentRules);
            return strategy;
        }


        public static Strategy SymbolCandidateNamesToIgnore(this Strategy strategy, params string[] namesToIgnore)
        {
            strategy.GetCandidateRules.NamesToIgnore.AddRange(namesToIgnore);
            return strategy;
        }
    }
}
