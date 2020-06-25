using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class AttributeWithPropertyRule<TAttribute>
        : AttributeRule<TAttribute>
    {
        public AttributeWithPropertyRule(string propertyName, Type type, SymbolType symbolType = SymbolType.All)
        : base(symbolType)
        {
            var _ = propertyName ?? throw new InvalidOperationException("PropertyName cannot be null, use AttributeWithImpliedPropertyValue rule");
            PropertyName = propertyName;
            Type = type;
        }

        public string PropertyName { get; }
        public Type Type { get; }
    }

    public class AttributeWithPropertyValueRule<TAttribute, TValue>
        : AttributeWithPropertyRule<TAttribute>, IRuleGetValue<TValue>, IRuleGetValues<TValue>
    {
        public AttributeWithPropertyValueRule(string propertyName, SymbolType symbolType = SymbolType.All)
            : base(propertyName, typeof(TValue), symbolType)
        { }

        public (bool success, TValue value) GetFirstOrDefaultValue(ISymbolDescriptor symbolDescriptor,
                                                                   IEnumerable<object> traits,
                                                                   ISymbolDescriptor parentSymbolDescriptor)
        {
            var values = GetAllValuesInternal(symbolDescriptor, traits, parentSymbolDescriptor);
            return values.Any()
                    ? (true, values.FirstOrDefault())
                    : (false, default);
        }

        public IEnumerable<TValue> GetAllValues(ISymbolDescriptor symbolDescriptor, IEnumerable<object> traits, ISymbolDescriptor parentSymbolDescriptor)
            => GetAllValuesInternal(symbolDescriptor, traits, parentSymbolDescriptor);

        // This might be the wrong return value
        private protected IEnumerable<TValue> GetAllValuesInternal(ISymbolDescriptor symbolDescriptor,
                                                                   IEnumerable<object> traits,
                                                                   ISymbolDescriptor parentSymbolDescriptor)
        {
            DescriptorMakerSpecificSourceBase tools = DescriptorMakerSpecificSourceBase.Tools;
            var matchingTraits = GetMatches(symbolDescriptor, traits, parentSymbolDescriptor);
            if (!matchingTraits.Any())
            {
                return Enumerable.Empty<TValue>();
            }

            var fromAllTraits = matchingTraits.SelectMany(trait => DescriptorMakerSpecificSourceBase.Tools.GetAllValues<TAttribute, TValue>(PropertyName,
                                    symbolDescriptor, trait, parentSymbolDescriptor))
                                 .ToList();
            return fromAllTraits;
        }

        public override string RuleDescription<TIRuleSet>()
            => $"If there is an attribute named '{typeof(TAttribute).Name}', its '{PropertyName}' property, with type {typeof(TValue)}";


    }
}
