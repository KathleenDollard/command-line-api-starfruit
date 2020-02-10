using System.Collections.Generic;
using System.CommandLine.ReflectionModel;
using System.Linq;
using System.Reflection;
using System.Xml;



namespace System.CommandLine.ReflectionModel
{
    public class IsRequiredStrategies
    {
        internal readonly BoolSymbolTypeAttributeStrategies AttributeStrategies = new BoolSymbolTypeAttributeStrategies();

        public bool? IsRequired(ParameterInfo parameterInfo, SymbolType symbolType) 
            => AttributeStrategies.AreAnyTrue(parameterInfo, symbolType);

        public bool? IsRequired(PropertyInfo propertyInfo, SymbolType symbolType)
            => AttributeStrategies.AreAnyTrue(propertyInfo, symbolType);

     }

    public static class RequiredStrategiesExtensions
    {
        public static IsRequiredStrategies AllStandard(this IsRequiredStrategies isRequiredStrategies)
               => isRequiredStrategies.HasStandardAttribute();

        public static IsRequiredStrategies HasAttribute<T>(this IsRequiredStrategies argStrategies, Func<T, bool> extractFunc, Func<SymbolType, bool> filterFunc = null)
            where T : Attribute
        {

            argStrategies.AttributeStrategies.Add<T>(a => extractFunc(a), filterFunc);
            return argStrategies;
        }

        public static IsRequiredStrategies HasAttribute<T>(this IsRequiredStrategies argStrategies, Func<SymbolType, bool> filterFunc = null)
                where T : Attribute
        {

            argStrategies.AttributeStrategies.Add<T>(filterFunc);
            return argStrategies;
        }

        public static IsRequiredStrategies HasStandardAttribute(this IsRequiredStrategies argStrategies)
            => argStrategies
                    .HasAttribute<CmdRequiredAttribute>()
                    .HasAttribute<CmdOptionAttribute>(a => a.OptionRequired, s => s == SymbolType.Option)
                    .HasAttribute<CmdOptionAttribute>(a => a.ArgumentRequired, s => s == SymbolType.Argument)
                    .HasAttribute<CmdArgumentAttribute>(a => a.Required);



    }
}
