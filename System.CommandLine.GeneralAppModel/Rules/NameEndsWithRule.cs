namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This class exists to give a better name for the StringContentsRule
    /// </summary>
    public class NameEndsWithRule : StringContentsRule
    {
        public NameEndsWithRule(string compareTo,
                                SymbolType symbolType = SymbolType.All)
             : base(StringPosition.EndsWith , compareTo, symbolType) { }

        protected override string NameOrString => "Name";
    }
}
