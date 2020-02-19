using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{
    public class NameStrategies
    {
        internal readonly StringAttributeStrategies AttributeStrategies = new StringAttributeStrategies();

        public string Name(ParameterInfo parameterInfo, SymbolType symbolType)
            => GetName(parameterInfo.GetCustomAttributes(),parameterInfo.Name, symbolType);
        public string Name(PropertyInfo propertyInfo, SymbolType symbolType)
            => GetName(propertyInfo.GetCustomAttributes(), propertyInfo.Name,symbolType);
        public string Name(Type type, SymbolType symbolType)
            => GetName(type.GetCustomAttributes(),  type.Name, symbolType);

        private string GetName(IEnumerable<Attribute> attributes, string defaultValue, SymbolType symbolType)
        {
            // order does matter here, attributes win over Xml Docs
            var (_, name) = AttributeStrategies.GetFirstValue(attributes, symbolType);

            return name is null
               ? defaultValue
               : name;
        }

    }

    public static class NameStrategiesExtensions
    {
        public static NameStrategies AllStandard(this NameStrategies nameStrategies)
               => nameStrategies.HasStandardAttributes();

        public static NameStrategies HasStandardAttributes(this NameStrategies nameStrategies)
        {
            nameStrategies.AttributeStrategies.Add<CmdNameAttribute>(a => ((CmdNameAttribute)a).Name);
            nameStrategies.AttributeStrategies.Add<CmdArgOptionBaseAttribute>(a => ((CmdArgOptionBaseAttribute)a).Name);
            nameStrategies.AttributeStrategies.Add<CmdCommandAttribute>(a => ((CmdCommandAttribute)a).Name);
            nameStrategies.AttributeStrategies.Add<CmdArgOptionBaseAttribute>(a => ((CmdArgOptionBaseAttribute)a).Name);
            return nameStrategies;
        }

    }

}