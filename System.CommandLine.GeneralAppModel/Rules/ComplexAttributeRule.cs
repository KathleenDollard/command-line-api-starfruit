using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{

    /// <summary>
    /// This type of rule supports attributes with multiple properties. The first usage was Arity. 
    /// A dictionary is returned, which contains the _Name_, not the _PropertyName_ of each expected item.
    /// The Name is consistent, the PropertyName can be whatever that particular strategy wants. 
    /// </summary>
    public class ComplexAttributeRule : NamedAttributeRule, IRuleGetValue<Dictionary<string, object>>
    {

        public ComplexAttributeRule(string attributeName, SymbolType symbolType = SymbolType.All)
            : base(attributeName, symbolType)
        {
        }
        public IEnumerable<NameAndType> PropertyNamesAndTypes { get; set; }

        public new(bool success, Dictionary<string, object> value) GetFirstOrDefaultValue(
                  SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var attributes = GetMatches(symbolDescriptor, items, parentSymbolDescriptor)
                                .OfType<Attribute>()
                                .ToList();
            if (attributes.Any(a => HasAtLeastOneProperty(a)))
            {
                var dictionary = new Dictionary<string, object>();
                foreach (var attribute in attributes)
                {
                    var propertyInfos = attribute.GetType().GetProperties();
                    foreach (var nameAndType in PropertyNamesAndTypes)
                    {
                        var info = propertyInfos.Where(p => p.Name == nameAndType.PropertyName).FirstOrDefault();
                        if (info != null)
                        {
                            var value = info.GetValue(attribute);
                            dictionary.Add(nameAndType.Name, value);
                        }
                    }
                }
                return (true, dictionary);
            }
            return (false, default);
        }

        private bool HasAtLeastOneProperty(Attribute attribute)
        {
            var propertyNames = PropertyNamesAndTypes.Select(p => p.PropertyName);
            return attribute.GetType().GetProperties().Any(p => propertyNames.Contains(p.Name));
        }

        public override string RuleDescription<TIRuleSet>()
            => $"If there is an attribute named '{AttributeName}': {string.Join(", ", PropertyNamesAndTypes.Select(p => ReportNameAndType(p)))}";

        private string ReportNameAndType(NameAndType p)
        {
            return $"{p.PropertyName } as {p.PropertyType }";
        }


        public class NameAndType
        {
            public NameAndType(string name, string propertyName, Type propertyType)
            {
                PropertyName = propertyName;
                PropertyType = propertyType;
                Name = name;
            }
            public string Name { get; set; }
            public string PropertyName { get; set; }
            public Type PropertyType { get; set; }
        }
    }
}
