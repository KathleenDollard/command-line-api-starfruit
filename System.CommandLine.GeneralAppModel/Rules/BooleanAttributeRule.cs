using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This rule supports attributes where the presence of the attribute with 
    /// no argument indicates true, and an optional property can either confirm 
    /// true or indicate false.
    /// </summary>
    public class BooleanAttributeRule<TAttribute> : AttributeRule<TAttribute>, IRuleGetValue<bool>
    {
        public BooleanAttributeRule(string propertyName = "", SymbolType symbolType = SymbolType.All)
        : base(symbolType)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }


        public (bool success, bool value) GetFirstOrDefaultValue(ISymbolDescriptor symbolDescriptor,
                                                                   IEnumerable<object> traits,
                                                                   ISymbolDescriptor parentSymbolDescriptor)
        {
            DescriptorMakerSpecificSourceBase tools = DescriptorMakerSpecificSourceBase.Tools;
            var matchingTraits = GetMatches(symbolDescriptor, traits, parentSymbolDescriptor);
            if (!matchingTraits.Any())
            {
                return (false, default);
            }
            if (string.IsNullOrEmpty(PropertyName))
            {
                // This is cool, we'll take in this order "Value", and the first
                var complexValues = matchingTraits.SelectMany(trait =>
                                             tools.GetComplexValue<TAttribute, object>(symbolDescriptor, trait, parentSymbolDescriptor));
                return complexValues.Count() switch
                {
                    // If a trait is found, but no property says otherwise, set to true
                    0 => (true, true),
                    1 => (true, GetValueOrFirst(complexValues)),
                    _ => throw new InvalidOperationException("If no property name is specified, there can be only one public gettable property on the attribute")
                };
            }

            var fromAllTraits = matchingTraits.SelectMany(trait => DescriptorMakerSpecificSourceBase.Tools.GetAllValues<TAttribute, bool>(PropertyName,
                                    symbolDescriptor, trait, parentSymbolDescriptor))
                                 .ToList();
            return fromAllTraits.Any()
                    ? (true, fromAllTraits.First())
                    : (false, default);

            static bool GetValueOrFirst(IEnumerable<(string key, object value)> complexValues)
            {
                var valueValues = complexValues.Where(x => x.key == "Value");
                return valueValues.Any()
                        ? (bool)valueValues.First().value
                        : (bool)complexValues.First().value;
            }
        }

        public override string RuleDescription<TIRuleSet>()
            => $"If there is an attribute named '{typeof(TAttribute).NameWithGenericArguments()}', its '{PropertyName}' property, with type {typeof(bool)}";


    }
}
