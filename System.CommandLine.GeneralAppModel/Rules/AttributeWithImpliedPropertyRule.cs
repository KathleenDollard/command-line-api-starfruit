using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This rule is for attributes that have a single property and when you use the rule
    /// you don't want to declare what that property might be named. This is useful for 
    /// rules for Description or Name, for example.
    /// </summary>
    public  class AttributeWithImpliedPropertyRule<TValue> : AttributeRule, IRuleGetValue<TValue>, IRuleGetValues<TValue>
    {
        public AttributeWithImpliedPropertyRule(string attributeName,  SymbolType symbolType = SymbolType.All)
        : base(attributeName, symbolType)
        {  }

        public (bool success, TValue value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                   IEnumerable<object> traits,
                                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var values = GetAllValuesInternal(symbolDescriptor, traits, parentSymbolDescriptor);
            return values.Any()
                    ? (true, values.FirstOrDefault())
                    : (false, default);
        }

        public IEnumerable<TValue> GetAllValues(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> traits, SymbolDescriptorBase parentSymbolDescriptor)
            => GetAllValuesInternal(symbolDescriptor, traits, parentSymbolDescriptor);

        // This might be the wrong return value
        private protected IEnumerable<TValue> GetAllValuesInternal(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> traits, SymbolDescriptorBase parentSymbolDescriptor)
        {
            SpecificSource tools = SpecificSource.Tools;
            var matchingTraits = GetMatches(symbolDescriptor, traits, parentSymbolDescriptor);
            if (!matchingTraits.Any())
            {
                return Enumerable.Empty<TValue>();
            }

            var complexValues = matchingTraits.SelectMany(trait =>
                                         tools.GetComplexValue<object>(AttributeName, symbolDescriptor, trait, parentSymbolDescriptor));
            return complexValues.Count() switch
            {
                // If a trait is found, but no property says otherwise, set to true
                0 => typeof(TValue) == typeof(bool)
                        ? new List<TValue> { (TValue)(object)true }
                        : Enumerable.Empty<TValue>(),
                1 => new List<TValue> { (TValue)complexValues.First().value },
                _ => throw new InvalidOperationException("If no property name is specified, there can be only one public gettable property on the attribute")
            };

        }

        public override string RuleDescription<TIRuleSet>()
            => $"If there is an attribute named '{AttributeName}', its first property, with type {typeof(TValue)}";


    }
}
