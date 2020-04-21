namespace System.CommandLine.GeneralAppModel
{
    public class NameRule : StringContentsRule
    {
        public NameRule(StringPosition position,
                        string compareTo,
                        SymbolType symbolType = SymbolType.All)
     : base(position, compareTo, symbolType) { }
    }

    public class NamePatternRule : StringContentsRule
    {
        public NamePatternRule(StringPosition position,
                        string compareTo,
                        SymbolType symbolType = SymbolType.All)
             : base(position, compareTo, symbolType) { }
    }
}
