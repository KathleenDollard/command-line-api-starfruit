﻿using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;

namespace System.CommandLine.NamedAttributeRules
{
    public class NamedAttributeWithOptionalValueRule<TValue> : NamedAttributeWithPropertyValueRule<TValue>, IRuleOptionalValue<TValue>
    {
        public NamedAttributeWithOptionalValueRule(string attributeName, string propertyName, SymbolType symbolType = SymbolType.All)
            : base(attributeName, propertyName, symbolType)
        { }

        public (bool success, TValue value) GetOptionalValue(ISymbolDescriptor symbolDescriptor,
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
                // This is currently legal for attributes with a single property or boolean. It might cause confusion.
                var allValuesInTrait = matchingTraits.SelectMany(trait =>
                                             tools.GetComplexValue<object>(AttributeName, symbolDescriptor, trait, parentSymbolDescriptor));
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

            var fromAllTraits = matchingTraits.SelectMany(trait => DescriptorMakerSpecificSourceBase.Tools.GetAllValues<TValue>
                                    (AttributeName, PropertyName, symbolDescriptor, trait, parentSymbolDescriptor))
                                    .ToList();
            return fromAllTraits.Any()
                    ? (true, fromAllTraits.First())
                    : (false, default);
        }

        public override string RuleDescription<TIRuleSet>()
             => $"If there is an attribute named '{AttributeName}' with a property '{PropertyName}', inlcude it as a {typeof(TValue).Name}";


    }
}
