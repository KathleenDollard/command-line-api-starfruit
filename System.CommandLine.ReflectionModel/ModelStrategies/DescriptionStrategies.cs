using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace System.CommandLine.ReflectionModel
{
    public class DescriptionStrategies
    {
        private readonly List<Func<XmlDocument, string>> xmlDocStrategies = new List<Func<XmlDocument, string>>();
        internal readonly StringAttributeStrategies AttributeStrategies = new StringAttributeStrategies();

        // TODO: Add XmlDocStrategies

        public string Description(ParameterInfo parameterInfo, SymbolType symbolType)
            => GetDescription(parameterInfo.GetCustomAttributes(), symbolType);
        public string Description(PropertyInfo propertyInfo, SymbolType symbolType)
            => GetDescription(propertyInfo.GetCustomAttributes(), symbolType);
        public string Description(Type type, SymbolType symbolType)
            => GetDescription(type.GetCustomAttributes(), symbolType);

        private string GetDescription(IEnumerable<Attribute> attributes, SymbolType symbolType)
        {
            // order does matter here, attributes win over Xml Docs
            var (found, description) = AttributeStrategies.GetFirstValue(attributes, symbolType);

            return found
               ? description
               : null; // else look for XML documents
        }
    }

    public static class DescriptionStrategiesExtensions
    {
        public static DescriptionStrategies AllStandard(this DescriptionStrategies descriptionStrategies)
               => descriptionStrategies.HasStandardAttribute();


        public static DescriptionStrategies HasStandardAttribute(this DescriptionStrategies descriptionStrategies)
        {
            descriptionStrategies.AttributeStrategies.Add<DescriptionAttribute>(a => ((DescriptionAttribute)a).Description);
            descriptionStrategies.AttributeStrategies.Add<CmdCommandAttribute>(a => ((CmdCommandAttribute)a).Description);
            descriptionStrategies.AttributeStrategies.Add<CmdArgOptionBaseAttribute>(a => ((CmdArgOptionBaseAttribute)a).Description);
            return descriptionStrategies;
        }
    }
}