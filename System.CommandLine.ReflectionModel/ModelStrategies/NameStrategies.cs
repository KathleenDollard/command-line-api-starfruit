using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionModel.Strategies;
using System.Reflection;

namespace System.CommandLine.ReflectionModel.ModelStrategies
{
    public class NameStrategies : ModelStrategies
    {
        internal readonly StringAttributeStrategies AttributeStrategies = new StringAttributeStrategies();

        public string Name(ParameterInfo parameterInfo, SymbolType symbolType)
            => GetName(parameterInfo.GetCustomAttributes(), parameterInfo.Name, symbolType);
        public string Name(PropertyInfo propertyInfo, SymbolType symbolType)
            => GetName(propertyInfo.GetCustomAttributes(), propertyInfo.Name, symbolType);
        public string Name(Type type, SymbolType symbolType)
            => GetName(type.GetCustomAttributes(), type.Name, symbolType);

        private string GetName(IEnumerable<Attribute> attributes, string defaultValue, SymbolType symbolType)
        {
            // order does matter here, attributes win over Xml Docs
            var (_, name) = AttributeStrategies.GetFirstValue(attributes, symbolType);

            return name is null
               ? defaultValue
               : name;
        }

        public override IEnumerable<string> StrategyDescriptions
           => AttributeStrategies.StrategyDescriptions;

        public override void UseStandard()
        {
            AttributeStrategies.Add<CmdNameAttribute>(a => ((CmdNameAttribute)a).Name);
            AttributeStrategies.Add<CmdArgOptionBaseAttribute>(a => ((CmdArgOptionBaseAttribute)a).Name);
            AttributeStrategies.Add<CmdCommandAttribute>(a => ((CmdCommandAttribute)a).Name);
        }
    }
}