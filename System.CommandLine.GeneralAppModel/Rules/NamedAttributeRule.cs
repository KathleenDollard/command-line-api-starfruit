using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{
    public class NamedAttributeRule : RuleBase, ISymbolSelectionRule, IValueRule<string>
    {
        public NamedAttributeRule(string attributeName, string type, string propertyName, SymbolType symbolType = SymbolType.All)
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

        //protected override IEnumerable<object> GetMatchingItems(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        //{
        //    return GetMatchingAttributes(symbolDescriptor, items)
        //                .Select(a => GetProperty(a, PropertyName))
        //                .OfType<object>();
        //}

        //public override bool HasMatch(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        //{
        //    return GetMatchingItems(symbolDescriptor, items)
        //                    .OfType<Attribute>()
        //                    .Where(v => !v.Equals(default))
        //                    .Any();

        //}

        // ISymbolSelectionRule implementation
        public bool IsMatch(SymbolDescriptorBase symbolDescriptor,
                            SymbolType requestedSymbolType,
                            object item,
                            SymbolDescriptorBase parentSymbolDescriptor)
        {
            if (item is ICustomAttributeProvider attributeProvider)
            {
                var attribute = item switch
                {
                    Attribute a => GetMatchingAttributes(symbolDescriptor, GetAttributes(attributeProvider))
                                .Any(),
                    _ => false
                };
            }
            return false;
        }

        public IEnumerable<T> GetMatches<T>(SymbolDescriptorBase symbolDescriptor,
                                                      SymbolType requestedSymbolType,
                                                      IEnumerable<T> items,
                                                      SymbolDescriptorBase parentSymbolDescriptor)
            => items
                    .Where(item => IsMatch(symbolDescriptor, requestedSymbolType, item, parentSymbolDescriptor))
                    .Where(x => !x.Equals(default));

        private IEnumerable<Attribute> GetAttributes(ICustomAttributeProvider attributeProvider)
            => attributeProvider.GetCustomAttributes(Context.IncludeBaseClassAttributes)
                                        .OfType<Attribute>();


        // IValueRule<string>
        public string GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                             SymbolType requestedSymbolType,
                                             IEnumerable<object> item,
                                             SymbolDescriptorBase parentSymbolDescriptor)
        {
            var attribute = GetMatches(symbolDescriptor, requestedSymbolType, item, parentSymbolDescriptor)
                                .OfType<Attribute>()
                                .FirstOrDefault();
            return GetProperty<string>(attribute, PropertyName);
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAllValues(SymbolDescriptorBase symbolDescriptor,
                                                SymbolType requestedSymbolType,
                                                IEnumerable<object> item,
                                                SymbolDescriptorBase parentSymbolDescriptor)
            => GetMatches(symbolDescriptor, requestedSymbolType, item, parentSymbolDescriptor)
                                .OfType<Attribute>()
                                .Select(a => GetProperty<string>(a, PropertyName))
                                .Where(x => x != default)
                                .Distinct();


        // Support methods
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
                    .Where(a => NewMethod(AttributeName, a));

            static bool NewMethod(string attributeName, Attribute a)
            {
                var itemName = a.GetType().Name;
                return itemName.Equals(attributeName, StringComparison.OrdinalIgnoreCase)
                    || itemName.Equals(attributeName + "Attribute", StringComparison.OrdinalIgnoreCase);
            }
        }

    }
}
