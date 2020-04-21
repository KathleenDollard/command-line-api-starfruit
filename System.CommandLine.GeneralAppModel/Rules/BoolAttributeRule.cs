namespace System.CommandLine.GeneralAppModel
{
    public class BoolAttributeRule : AttributeRule<bool>
    {
        public BoolAttributeRule(string attributeName,  string propertyName = null, SymbolType symbolType = SymbolType.All)
            : base(attributeName, "bool", propertyName, symbolType)
        {
        }
    }
}
