namespace System.CommandLine.GeneralAppModel
{
    public class BoolAttributeRule : NamedAttributeRule
    {
        public BoolAttributeRule(string attributeName,  string propertyName = null, SymbolType symbolType = SymbolType.All)
            : base(attributeName, "bool", propertyName, symbolType)
        {
        }
    }
}
