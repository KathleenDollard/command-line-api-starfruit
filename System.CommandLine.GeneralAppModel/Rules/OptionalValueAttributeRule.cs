using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Rules
{
    public class OptionalValueAttributeRule <T>: ComplexAttributeRule, IRuleGetValue<T>
    {
        public OptionalValueAttributeRule(string attributeName, string propertyName, SymbolType symbolType = SymbolType.All) 
            : base(attributeName, symbolType)
        {
            PropertyNamesAndTypes = new NameAndType[] { new NameAndType("Value", propertyName , typeof(T)) };
        }

       public new  (bool success, T value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var asComplex = this as ComplexAttributeRule;
            var (success, dictionary) = asComplex.GetFirstOrDefaultValue(symbolDescriptor, items, parentSymbolDescriptor);
            if (success && dictionary.Any())
            {
                return Conversions.TryTo<T>(dictionary.First().Value);
            }
            return (false, default);
        }

        public override string RuleDescription<TIRuleSet>()
             => $"If there is an attribute named '{AttributeName}' with a property '{PropertyNamesAndTypes.First().PropertyName}', inlcude it as a {typeof(T).Name}";


    }
}
