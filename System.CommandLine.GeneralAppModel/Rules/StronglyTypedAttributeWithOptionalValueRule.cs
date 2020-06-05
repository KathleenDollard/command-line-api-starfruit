using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class StronglyTypedAttributeWithOptionalValueRule<TAttribute, TValue> : StronglyTypedAttributeWithPropertyValueRule<TAttribute, TValue>, IRuleOptionalValue<TValue>
    {
        public StronglyTypedAttributeWithOptionalValueRule(string propertyName, SymbolType symbolType = SymbolType.All)
            : base(propertyName, symbolType)
        { }

        public (bool success, TValue value) GetOptionalValue(ISymbolDescriptor symbolDescriptor,
                                                        IEnumerable<object> traits,
                                                        ISymbolDescriptor parentSymbolDescriptor)
        {
            SpecificSource tools = SpecificSource.Tools;
            var matchingTraits = GetMatches(symbolDescriptor, traits, parentSymbolDescriptor);
            if (!matchingTraits.Any())
            {
                return (false, default);
            }
            if (string.IsNullOrEmpty(PropertyName))
            {
                // This is currently legal for attributes with a single property or boolean. It might cause confusion.
                var allValuesInTrait = matchingTraits.SelectMany(trait =>
                                             tools.GetComplexValue<TAttribute, object>( symbolDescriptor, trait, parentSymbolDescriptor));
                return allValuesInTrait.Count() switch
                {
                    // If a trait is found, but no property says otherwise, set to true
                    0 => typeof(TValue) == typeof(bool)
                            ? (true, (TValue)(object)true)
                            : (false, (TValue)(object)true),
                    1 => (true, (TValue)allValuesInTrait.Single().value),
                    _ => throw new InvalidOperationException("If no property name is specified, there can be only one public gettable property on the attribute")
                };
            }

            var fromAllTraits = matchingTraits.SelectMany(trait => SpecificSource.Tools.GetAllValues<TAttribute, TValue>
                                    ( PropertyName, symbolDescriptor, trait, parentSymbolDescriptor))
                                    .ToList();
            return fromAllTraits.Any()
                    ? (true, fromAllTraits.First())
                    : (false, default);
        }

        public override string RuleDescription<TIRuleSet>()
             => $"If there is an attribute named '{typeof(TAttribute).Name}' with a property '{PropertyName}', inlcude it as a {typeof(TValue).Name}";


    }
}
