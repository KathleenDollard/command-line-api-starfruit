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

        protected override  IEnumerable<T> GetMatchingItems(SymbolType symbolType, object[] items)
        {
            return GetMatchingAttributes(symbolType, items)
                        .Select(a => GetProperty(a, PropertyName));
        }

        private IEnumerable<Attribute> GetMatchingAttributes(SymbolType symbolType, object[] items)
        {
            if (SymbolType != SymbolType.All && SymbolType != symbolType)
            {
                return Array.Empty<Attribute>();
            }
            return items
                    .OfType<Attribute>()
                    .Where(a => a.GetType().Name.Equals(AttributeName, StringComparison.OrdinalIgnoreCase));
        }

        private static T GetProperty(Attribute attribute, string propertyName)
        {
            var raw = attribute.GetType()
                              .GetProperty(propertyName)
                              .GetValue(attribute);
            return Conversions.To<T>(raw);
        }

        public override bool HasMatch(SymbolType symbolType, object[] items)
        {
            return GetMatchingItems(symbolType, items)
                            .OfType<Attribute>()
                            .Where(v=>!v.Equals(default))
                            .Any();

        }
    }
}
