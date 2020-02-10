using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{
    public class BoolAttributeStrategies : AttributeStrategies<bool?>
    {
        // For each attribute, a single value is returned. Unless an attribute can be placed multiple times, this will be single. 
        // If multiple attribute types are present, they can disagree. True wins.
        public void Add<T>(Func<T, bool?> getValue)
            where T : Attribute
            => Add((attributes) =>  AreAnyTrueNullIfNone(attributes
                                                        .OfType<T>()
                                                        .Select(a => getValue(a))));

        public void Add<T>()
             where T : Attribute
             => Add((attributes) =>  AreAnyTrueNullIfNone(attributes
                                                         .OfType<T>()
                                                         .Select(a => (bool?)true)));

        public bool AreAnyTrue(ParameterInfo parameterInfo)
            => AreAnyTrue(attributeStrategies
                            .Select(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>())));

        public bool AreAnyTrue(PropertyInfo propertyInfo)
            => AreAnyTrue(attributeStrategies
                     .Select(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>())));

        public bool AreAnyTrue(Type type)
            => AreAnyTrue(attributeStrategies
                     .Select(s => s(type.GetCustomAttributes().OfType<Attribute>())));

        private static bool AreAnyTrue(IEnumerable<bool?> values)
            => values
                            .Where(x => x.GetValueOrDefault())
                            .Any();

        public bool? AreAnyTrueNullIfNone(ParameterInfo parameterInfo)
            => AreAnyTrueNullIfNone(attributeStrategies
                    .Select(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>())));

        public bool? AreAnyTrueNullIfNone(PropertyInfo propertyInfo)
            => AreAnyTrueNullIfNone(attributeStrategies
                     .Select(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>())));

        public bool? AreAnyTrueNullIfNone(Type type)
            => AreAnyTrueNullIfNone(attributeStrategies
                     .Select(s => s(type.GetCustomAttributes().OfType<Attribute>())));


        private static bool? AreAnyTrueNullIfNone(IEnumerable<bool?> values)
            => values.Any(v => v.HasValue)
                        ? (bool?)values.Where(v => v.HasValue).Any(v => v.Value)
                        : null;
    }
}