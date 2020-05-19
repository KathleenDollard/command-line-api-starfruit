﻿using System.Collections.Generic;
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
            SpecificSource tools = SpecificSource.Tools;
            var attributes = GetMatches(symbolDescriptor, items, parentSymbolDescriptor)
                                .ToList();
            if (attributes.Any(a => tools.ComplexAttributeHasAtLeastOneProperty(PropertyNamesAndTypes, a)))
            {
                var dictionary = new Dictionary<string, object>();
                foreach (var attribute in attributes)
                {
                    foreach (var nameAndType in PropertyNamesAndTypes)
                    {
                        var (success, value) = SpecificSource.Tools.GetAttributePropertyValue(attribute, nameAndType.PropertyName);
                        if (success)
                        {
                            dictionary.Add(nameAndType.Name, value);
                        }
                    }
                }
                return (true, dictionary);
            }
            return (false, default);
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
