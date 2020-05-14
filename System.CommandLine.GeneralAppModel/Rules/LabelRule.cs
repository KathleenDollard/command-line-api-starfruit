namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LabelRule : RuleBase
    {
        public LabelRule(string label, SymbolType symbolType = SymbolType.All)
            : base(symbolType)
        {
            Label = label;
        }

        public string Label { get; }

        public override string RuleDescription
            => $"Label Rule: {Label}";
    }
}
