using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace System.CommandLine.ReflectionModel
{
    public class NameStrategies
    {
        internal readonly StringAttributeStrategies attributeStrategies = new StringAttributeStrategies();

        public string Name(ParameterInfo parameterInfo) 
            => attributeStrategies.GetFirstNonNullOrDefaultValue(parameterInfo, parameterInfo.Name);

        public string Name(PropertyInfo propertyInfo)
                => attributeStrategies.GetFirstNonNullOrDefaultValue(propertyInfo, propertyInfo.Name);

        public string Name(Type type)
                  => attributeStrategies.GetFirstNonNullOrDefaultValue(type, type.Name);

    }

    public static class NameStrategiesExtensions
    {
        public static NameStrategies AllStandard(this NameStrategies nameStrategies)
               => nameStrategies.HasStandardAttribute();

        public static NameStrategies HasAttribute<T>(this NameStrategies argStrategies, Func<T, string> extractFunc)
            where T : Attribute
        {
            argStrategies.attributeStrategies.Add(extractFunc);
            return argStrategies;
        }

        public static NameStrategies HasStandardAttribute(this NameStrategies argStrategies)
            => argStrategies
                    .HasAttribute<CmdNameAttribute >(a => a.Name)
                    .HasAttribute<CmdArgOptionBaseAttribute>(a => a.Name)
                    .HasAttribute<CmdCommandAttribute>(a => a.Name)
                    .HasAttribute<CmdArgOptionBaseAttribute>(a => a.Name);

    }
}