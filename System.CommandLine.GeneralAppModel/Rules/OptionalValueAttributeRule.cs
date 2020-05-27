using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class OptionalValueAttributeRule <T>: ComplexAttributeRule
    {
        public OptionalValueAttributeRule(string attributeName, string propertyName, SymbolType symbolType = SymbolType.All) 
            : base(attributeName, symbolType)
        {
            PropertyNamesAndTypes = new NameAndType[] { new NameAndType("Value", propertyName , typeof(T)) };
        }

        public override string RuleDescription<TIRuleSet>()
             => $"If there is an attribute named '{AttributeName}' with a property '{PropertyNamesAndTypes.First().PropertyName}', inlcude it as a {typeof(T).Name}";


    }
}
