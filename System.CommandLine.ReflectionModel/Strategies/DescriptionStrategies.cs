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

        private readonly List<Func<IEnumerable<Attribute>, string>> attributeStrategies = new List<Func<IEnumerable<Attribute>, string>>();

        //public void AddNameStrategy(Func<string, bool> strategy)
        //    => nameStrategies.Add(strategy);

        public void AddAttributeStrategy(Func<IEnumerable<Attribute>, string> strategy)
         => attributeStrategies.Add(strategy);

        public string Description(ParameterInfo parameterInfo)
        {
            // order does matter here, attributes win over Xml Docs
            var descriptions = attributeStrategies
                                .Select(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>()))
                                .Where(s => !(s is null));
            return descriptions.Any()
                     ? descriptions.First()
                     : // else look for XML documents
                     null;
        }

        public string Description(PropertyInfo propertyInfo)
        {
            // order does matter here, attributes win over Xml Docs
            var descriptions = attributeStrategies
                                .Select(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>()))
                                .Where(s => !(s is null));
            return descriptions.Any()
                     ? descriptions.First()
                     : // else look for XML documents
                     null;
        }

        public string Description(Type type)
        {
            // order does matter here, attributes win over Xml Docs
            var descriptions = attributeStrategies
                                .Select(s => s(type.GetCustomAttributes().OfType<Attribute>()))
                                .Where(s => !(s is null));
            return descriptions.Any()
                     ? descriptions.First()
                     : // else look for XML documents
                     null;
        }
    }

    public static class DescriptionStrategiesExtensions
    {
        public static DescriptionStrategies AllStandard(this DescriptionStrategies descriptionStrategies)
               => descriptionStrategies.HasStandardAttribute();

        public static DescriptionStrategies HasAttribute<T>(this DescriptionStrategies argStrategies, Func<T, string> extractFunc)
            where T : Attribute
        {

            argStrategies.AddAttributeStrategy(
                attributes => attributes
                                .OfType<T>()
                                .Select(a => extractFunc(a))
                                .FirstOrDefault());
            return argStrategies;
        }

        public static DescriptionStrategies HasStandardAttribute(this DescriptionStrategies argStrategies)
            => argStrategies
                    .HasAttribute<DescriptionAttribute>(a => a.Description)
                    .HasAttribute<CmdCommandAttribute>(a => a.Description)
                    .HasAttribute<CmdArgOptionBaseAttribute>(a => a.Description);

    }
}