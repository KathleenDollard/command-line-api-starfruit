using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class AttributeWithPropertyRule : AttributeRule
    {
        public AttributeWithPropertyRule(string attributeName, string propertyName, Type type, SymbolType symbolType = SymbolType.All)
        : base(attributeName, symbolType)
        {
            PropertyName = propertyName;
            Type = type;
        }

        public string PropertyName { get; }
        public Type Type { get; }
    }

    public class AttributeWithPropertyRule<TValue> : AttributeWithPropertyRule, IRuleGetValue<TValue>, IRuleGetValues<TValue>
    {
        public AttributeWithPropertyRule(string attributeName, string propertyName, SymbolType symbolType = SymbolType.All)
            : base(attributeName, propertyName, typeof(TValue), symbolType)
        { }

        public AttributeWithPropertyRule(string attributeName, SymbolType symbolType = SymbolType.All)
            : base(attributeName, null, typeof(TValue), symbolType)
        { }


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
        private IEnumerable<TValue> GetAllValuesInternal(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> traits, SymbolDescriptorBase parentSymbolDescriptor)
        {
            SpecificSource tools = SpecificSource.Tools;
            var matchingTraits = GetMatches(symbolDescriptor, traits, parentSymbolDescriptor);
            if (!matchingTraits.Any())
            {
                return Enumerable.Empty<TValue>();
            }
            if (string.IsNullOrEmpty(PropertyName))
            {
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

            var fromAllTraits = matchingTraits.SelectMany(trait => SpecificSource.Tools.GetAllValues<TValue>(AttributeName, PropertyName,
                                    symbolDescriptor, trait, parentSymbolDescriptor))
                                 .ToList();
            return fromAllTraits;
        }

        public override string RuleDescription<TIRuleSet>()
            => $"If there is an attribute named '{AttributeName}', its '{PropertyName}' property, with type {typeof(TValue)}";


    }
}
