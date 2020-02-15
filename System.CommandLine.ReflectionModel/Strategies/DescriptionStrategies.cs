using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace System.CommandLine.ReflectionModel
{
    public class DescriptionStrategies
    {
        private readonly List<Func<XmlDocument, string>> xmlDocStrategies = new List<Func<XmlDocument, string>>();
        internal readonly StringAttributeStrategies attributeStrategies = new StringAttributeStrategies();

        // TODO: Add XmlDocStrategies


        public string Description(ParameterInfo parameterInfo)
        {
            // order does matter here, attributes win over Xml Docs
            var description = attributeStrategies.GetFirstNonNullOrDefaultValue(parameterInfo, null);

            return description == null
                     ? null // else look for XML documents
                     : description;
        }

        public string Description(PropertyInfo propertyInfo)
        {
            // order does matter here, attributes win over Xml Docs
            var description = attributeStrategies.GetFirstNonNullOrDefaultValue(propertyInfo, null);

            return description == null
                     ? null // else look for XML documents
                     : description;

        }

        public string Description(Type type)
        {
            var description = attributeStrategies.GetFirstNonNullOrDefaultValue(type, null);

            return description == null
                     ? null // else look for XML documents
                     : description;

        }
    }

    public static class DescriptionStrategiesExtensions
    {
        public static DescriptionStrategies AllStandard(this DescriptionStrategies descriptionStrategies)
               => descriptionStrategies.HasStandardAttribute();

        public static DescriptionStrategies HasAttribute<T>(this DescriptionStrategies argStrategies, Func<T, string> extractFunc)
            where T : Attribute
        {

            argStrategies.attributeStrategies.Add(extractFunc);
            return argStrategies;
        }

        public static DescriptionStrategies HasStandardAttribute(this DescriptionStrategies argStrategies)
            => argStrategies
                    .HasAttribute<DescriptionAttribute>(a => a.Description)
                    .HasAttribute<CmdCommandAttribute>(a => a.Description)
                    .HasAttribute<CmdArgOptionBaseAttribute>(a => a.Description);

    }
}