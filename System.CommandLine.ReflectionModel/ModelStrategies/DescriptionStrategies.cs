using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionModel.Strategies;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel.ModelStrategies
{
    public class DescriptionStrategies : ModelStrategies
    {
        private XmlDocStrategy xmlDocStrategy = null;
        internal readonly StringAttributeStrategies AttributeStrategies = new StringAttributeStrategies();

        public void AddXmlDocumentStrategy() 
            => xmlDocStrategy = new XmlDocStrategy();

        public string Description(ParameterInfo parameterInfo, SymbolType symbolType)
            => GetDescription(parameterInfo.Name, parameterInfo.Member, parameterInfo.GetCustomAttributes(), symbolType);
        public string Description(PropertyInfo propertyInfo, SymbolType symbolType)
            => GetDescription(propertyInfo.Name, propertyInfo, propertyInfo.GetCustomAttributes(), symbolType);
        public string Description(Type type, SymbolType symbolType)
            => GetDescription(type.Name, type, type.GetCustomAttributes(), symbolType);

        private (bool found, string description) GetAttributeDescription(IEnumerable<Attribute> attributes, SymbolType symbolType)
            => AttributeStrategies.GetFirstValue(attributes, symbolType);

        private string GetDescription(string name, object info, IEnumerable<Attribute> attributes, SymbolType symbolType)
        {
            // order does matter here, attributes win over Xml Docs
            var (found, description) = GetAttributeDescription(attributes, symbolType);
            if (found)
            {
                return description;
            }

            if (!(xmlDocStrategy is null))
            {
                (found, description) = xmlDocStrategy.GetDescription(info, name);
                if (found)
                {
                    return description;
                }
            }
            return null;
        }

        public override IEnumerable<string> StrategyDescriptions
        {
            get
            {
                var descriptions = AttributeStrategies.StrategyDescriptions;
                return xmlDocStrategy is null
                    ? descriptions
                    : descriptions.Union(new[] { xmlDocStrategy.StrategyDescription });
            }
        }

        public override void UseStandard()
        {
            AttributeStrategies.Add<DescriptionAttribute>(a => ((DescriptionAttribute)a).Description);
            AttributeStrategies.Add<CmdCommandAttribute>(a => ((CmdCommandAttribute)a).Description);
            AttributeStrategies.Add<CmdArgOptionBaseAttribute>(a => ((CmdArgOptionBaseAttribute)a).Description);
            AddXmlDocumentStrategy();
        }
    }

  

}