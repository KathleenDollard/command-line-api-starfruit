namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This rule allows either the presence of the attribute or explicitly setting a property to indicate true
    /// </summary>
    public class BoolAttributeRule : NamedAttributeRule
    {
        public BoolAttributeRule(string attributeName, string propertyName = null, SymbolType symbolType = SymbolType.All)
            : base(attributeName, "bool", propertyName, symbolType)
        { }
        public override string RuleDescription
           => $"Bool Attribute Rule: {AttributeName} PropertyName: {PropertyName}";
    }
}
