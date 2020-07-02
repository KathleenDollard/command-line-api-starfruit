using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;

namespace System.CommandLine.NamedAttributeRules
{

    /// <summary>
    /// This type of rule supports attributes with multiple properties. The first usage was Arity. 
    /// A dictionary is returned, which contains the _Name_, not the _PropertyName_ of each expected item.
    /// The Name is consistent, the PropertyName can be whatever that particular strategy wants. 
    /// </summary>
    public class NamedAttributeWithComplexValueRule : NamedAttributeRule, IRuleGetValue<Dictionary<string, object>>
    {

        public NamedAttributeWithComplexValueRule(string attributeName, SymbolType symbolType = SymbolType.All)
            : base(attributeName, symbolType)
        {
        }

        public List<NameAndType> PropertyNamesAndTypes { get; } = new List<NameAndType>();

        public (bool success, Dictionary<string, object> value) GetFirstOrDefaultValue(ISymbolDescriptor symbolDescriptor,
                                                                                       IEnumerable<object> traits,
                                                                                       ISymbolDescriptor parentSymbolDescriptor)
        {
            DescriptorMakerSpecificSourceBase tools = DescriptorMakerSpecificSourceBase.Tools;
            var matchingTraits = GetMatches(symbolDescriptor, traits, parentSymbolDescriptor).ToList();
            var complexValues = matchingTraits.SelectMany(trait =>
                    tools.GetComplexValue<object>(AttributeName, symbolDescriptor, trait, parentSymbolDescriptor)
                            .Where(keyPair => PropertyNamesAndTypes.Any(nameAndType => nameAndType.PropertyName == keyPair.key))
                    );

            return complexValues.Any()
                ? (true, complexValues.ToDictionary(pair => pair.key, pair => pair.value))
                : (false, new Dictionary<string, object>());
            ;
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
