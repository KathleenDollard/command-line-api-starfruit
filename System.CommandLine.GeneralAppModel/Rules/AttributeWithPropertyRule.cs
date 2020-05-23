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

    public class NamedAttributeWithPropertyRule<TValue> : AttributeWithPropertyRule, IRuleGetValue<TValue>, IRuleGetValues<TValue>
    {
        public NamedAttributeWithPropertyRule(string attributeName, string propertyName, SymbolType symbolType = SymbolType.All)
            : base(attributeName, propertyName, typeof(TValue), symbolType)
        { }

        public (bool success, TValue value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                   IEnumerable<object> traits,
                                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var values = GetAllValuesInternal(symbolDescriptor, traits, parentSymbolDescriptor);
            return values.Any()
                    ? (true,values.FirstOrDefault())
                    : (false, default);
        }

        public IEnumerable<TValue> GetAllValues(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> traits, SymbolDescriptorBase parentSymbolDescriptor) 
            => GetAllValuesInternal(symbolDescriptor, traits, parentSymbolDescriptor);

        // This might be the wrong return value
        private IEnumerable<TValue> GetAllValuesInternal(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> traits, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var matchingTraits = GetMatches(symbolDescriptor, traits, parentSymbolDescriptor);
            var fromAllTraits = matchingTraits.SelectMany(trait => SpecificSource.Tools.GetAllValues<TValue>(AttributeName, PropertyName,
                                    symbolDescriptor, trait, parentSymbolDescriptor))
                                 .ToList();
            return fromAllTraits;
        }

        public override string RuleDescription<TIRuleSet>()
            => $"If there is an attribute named '{AttributeName}', its '{PropertyName}' property, with type {typeof(TValue)}";

 
    }
}
