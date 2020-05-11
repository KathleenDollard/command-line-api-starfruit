using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class AttributeRule<T> : RuleBase<T>
    {
        public AttributeRule(string attributeName, string type, string propertyName, SymbolType symbolType = SymbolType.All)
        : base(symbolType)
        {
            AttributeName = attributeName;
            Type = type;
            PropertyName = propertyName;
        }

        public string AttributeName { get; set; }
        public string Type { get; set; }
        public string PropertyName { get; set; }
        public override string RuleDescription { get; }

        protected override IEnumerable<object> GetMatchingItems(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            return GetMatchingAttributes(symbolDescriptor, items)
                        .Select(a => GetProperty(a, PropertyName))
                        .OfType<object>();
        }

        private IEnumerable<Attribute> GetMatchingAttributes(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            return SymbolType != SymbolType.All && SymbolType != symbolDescriptor.SymbolType
                ? Array.Empty<Attribute>()
                : items
                    .OfType<Attribute>()
                    .Where(a => NewMethod(AttributeName, a));

            static bool NewMethod(string attributeName, Attribute a)
            {
                var itemName = a.GetType().Name;
                return itemName.Equals(attributeName, StringComparison.OrdinalIgnoreCase)
                    || itemName.Equals(attributeName + "Attribute", StringComparison.OrdinalIgnoreCase) ;
            }
        }

        private static T GetProperty(Attribute attribute, string propertyName)
        {
            var raw = attribute.GetType()
                              .GetProperty(propertyName)
                              .GetValue(attribute);
            return Conversions.To<T>(raw);
        }

        public override bool HasMatch(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            return GetMatchingItems(symbolDescriptor, items)
                            .OfType<Attribute>()
                            .Where(v => !v.Equals(default))
                            .Any();

        }
    }
}
