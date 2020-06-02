namespace System.CommandLine.GeneralAppModel
{
    public static class FullStrategy
    {
        public static Strategy SetFullRules(this Strategy strategy)
        {
            strategy.SelectSymbolRules.SetFullSelectSymbolRules();
            strategy.GetCandidateRules.SetFullCandidatesRules();
            strategy.ArgumentRules.SetFullArgumentRules();
            strategy.CommandRules.SetFullCommandRules();
            strategy.OptionRules.SetFullOptionRules();
            strategy.OptionArgumentRules.SetFullOptionArgumentRules();
            return strategy;
        }


        public static Strategy SymbolCandidateNamesToIgnore(this Strategy strategy, params string[] namesToIgnore)
        {
            strategy.GetCandidateRules.NamesToIgnore.AddRange(namesToIgnore);
            return strategy;
        }
    }
}
