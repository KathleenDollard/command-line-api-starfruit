using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{
    public class StringAttributeStrategies : AttributeStrategies<string>
    {

        public void Add<T>(Func<T, string> getValue)
            where T : Attribute
            => Add(attributes => attributes
                                .OfType<T>()
                                .Select(a => getValue(a))
                                .Where(x=>!(x is null))
                                .FirstOrDefault());


        public string GetFirstNonNullOrDefaultValue(ParameterInfo parameterInfo, string defaultValue)
        {
            var values = attributeStrategies
                                 .Select(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>()))
                                 .Where(x => !(x is null));
            return values.Any()
                     ? values.First()
                     : defaultValue;
        }

        public string GetFirstNonNullOrDefaultValue(PropertyInfo propertyInfo, string defaultValue)
        {
            var values = attributeStrategies
                                 .Select(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>()))
                                 .Where(x => !(x is null));
            return values.Any()
                     ? values.First()
                     : defaultValue;
        }

        public string GetFirstNonNullOrDefaultValue(Type type, string defaultValue)
        {
            var values = attributeStrategies
                                 .Select(s => s(type.GetCustomAttributes().OfType<Attribute>()))
                                 .Where(x => !(x is null));
            return values.Any()
                     ? values.First()
                     : defaultValue;
        }


    }
}