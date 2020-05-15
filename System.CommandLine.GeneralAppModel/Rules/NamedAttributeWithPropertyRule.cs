using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Rules;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{
    //public class NamedAttributeWithPropertyRule<TValue> : RuleBase, IRuleGetValue<TValue>, IRuleGetValues<TValue>
    public class NamedAttributeWithPropertyRule<TValue> : AttributeRuleBase<TValue>, IRuleGetValue<TValue>
    {
        public NamedAttributeWithPropertyRule(string attributeName, string propertyName, SymbolType symbolType = SymbolType.All)
        : base(attributeName,symbolType)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get;  }

        public override (bool success, TValue value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                   IEnumerable<object> item,
                                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var attributes = GetMatches(symbolDescriptor, item, parentSymbolDescriptor)
                                .OfType<Attribute>();
            if (attributes.Any())
            {
                return (true, GetProperty<TValue>(attributes.FirstOrDefault(), PropertyName));
            }
            return (false, default);
        }

        private static T GetProperty<T>(Attribute attribute, string propertyName)
        {
            var raw = attribute.GetType()
                              .GetProperty(propertyName)
                              .GetValue(attribute);
            return Conversions.To<T>(raw);
        }

        private IEnumerable<Attribute> GetMatchingAttributes(SymbolDescriptorBase symbolDescriptor, IEnumerable<Attribute> items)
        {
            return SymbolType != SymbolType.All && SymbolType != symbolDescriptor.SymbolType
                ? Array.Empty<Attribute>()
                : items
                    .Where(a => DoesAttributeMatch(AttributeName, a));
        }

        private IEnumerable<Attribute> GetAttributes(ICustomAttributeProvider attributeProvider)
            => attributeProvider.GetCustomAttributes(Context.IncludeBaseClassAttributes)
                                        .OfType<Attribute>();

        public override string RuleDescription<TIRuleSet>()
            => $"If there is an attribute named '{AttributeName}', its '{PropertyName}' property";
    }
}
