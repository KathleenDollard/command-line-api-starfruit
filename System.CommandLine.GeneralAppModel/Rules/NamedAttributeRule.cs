using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{
    public class NamedAttributeRule : RuleBase, IRuleGetValue<string>, IRuleGetValues<string>
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

        public (bool success, string value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                   IEnumerable<object> item,
                                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var attributes = GetMatches(symbolDescriptor, item, parentSymbolDescriptor)
                                .OfType<Attribute>();
            if (attributes.Any())
            {
                return (true, GetProperty<string>(attributes.FirstOrDefault(), PropertyName));
            }
            return (false, default);
        }

        public IEnumerable<string> GetAllValues(SymbolDescriptorBase symbolDescriptor,
                                                IEnumerable<object> item,
                                                SymbolDescriptorBase parentSymbolDescriptor)
        {
            return GetMatches(symbolDescriptor, item, parentSymbolDescriptor)
                                     .OfType<Attribute>()
                                     .Select(a => GetProperty<string>(a, PropertyName))
                                     .Where(x => x != default)
                                     .Distinct();
        }

        public IEnumerable<T> GetMatches<T>(SymbolDescriptorBase symbolDescriptor,
                                                     IEnumerable<T> items,
                                                     SymbolDescriptorBase parentSymbolDescriptor)
           => items
                   .Where(item => IsMatch(symbolDescriptor, item, parentSymbolDescriptor))
                   .Where(x => !x.Equals(default));

        public bool IsMatch(SymbolDescriptorBase symbolDescriptor,
                             object item,
                             SymbolDescriptorBase parentSymbolDescriptor)
        {
            return item switch
            {
                Attribute a => DoesAttributeMatch(AttributeName, a),
                _ => false
            };
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

        private bool DoesAttributeMatch(string attributeName, Attribute a)
        {
            var itemName = a.GetType().Name;
            return itemName.Equals(attributeName, StringComparison.OrdinalIgnoreCase)
                || itemName.Equals(attributeName + "Attribute", StringComparison.OrdinalIgnoreCase);
        }

        private IEnumerable<Attribute> GetAttributes(ICustomAttributeProvider attributeProvider)
            => attributeProvider.GetCustomAttributes(Context.IncludeBaseClassAttributes)
                                        .OfType<Attribute>();

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



        //// IValueRule<string>
        //public string GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
        //                                     SymbolType requestedSymbolType,
        //                                     IEnumerable<object> item,
        //                                     SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    var attribute = GetMatches(symbolDescriptor, requestedSymbolType, item, parentSymbolDescriptor)
        //                        .OfType<Attribute>()
        //                        .FirstOrDefault();
        //    return GetProperty<string>(attribute, PropertyName);
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<string> GetAllValues(SymbolDescriptorBase symbolDescriptor,
        //                                        SymbolType requestedSymbolType,
        //                                        IEnumerable<object> item,
        //                                        SymbolDescriptorBase parentSymbolDescriptor)
        //    => GetMatches(symbolDescriptor, requestedSymbolType, item, parentSymbolDescriptor)
        //                        .OfType<Attribute>()
        //                        .Select(a => GetProperty<string>(a, PropertyName))
        //                        .Where(x => x != default)
        //                        .Distinct();


        //// Support methods



    }
}
