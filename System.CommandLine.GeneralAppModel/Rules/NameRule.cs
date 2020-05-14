namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This class exists to give a better name for the StringContentsRule
    /// </summary>
    public class NamePatternRule : StringContentsRule
    {
        public NamePatternRule(StringPosition position,
                        string compareTo,
                        SymbolType symbolType = SymbolType.All)
             : base(position, compareTo, symbolType) { }
        public override string RuleDescription
             => $"Name Pattern Rule: {Position} - '{CompareTo}'";
    }
}
