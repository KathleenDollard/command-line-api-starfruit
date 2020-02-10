using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{
    public class BoolSymbolTypeAttributeStrategies : AttributeStrategiesWithSymbolFilter<bool?>
    {
        // For each attribute, a single value is returned. Unless an attribute can be placed multiple times, this will be single. 
        // If multiple attribute types are present, they can disagree. True wins.
        public void Add<T>(Func<T, bool?> getValue, Func<SymbolType, bool> filterFunc = null)
            where T : Attribute
            => Add((attributes, symbolType) => filterFunc.Filter(symbolType)
                                               ? AreAnyTrueNullIfNone(attributes
                                                        .OfType<T>()
                                                        .Select(a => getValue(a)))
                                               : default);

        public void Add<T>(Func<SymbolType, bool> filterFunc = null)
             where T : Attribute
             => Add((attributes, symbolType) => filterFunc.Filter(symbolType)
                                               ? AreAnyTrueNullIfNone(attributes
                                                         .OfType<T>()
                                                         .Select(a => (bool?)true))
                                               : default);

        public bool AreAnyTrue(ParameterInfo parameterInfo, SymbolType symbolType)
            => AreAnyTrue(attributeStrategies
                            .Select(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>(), symbolType)));

        public bool AreAnyTrue(PropertyInfo propertyInfo, SymbolType symbolType)
            => AreAnyTrue(attributeStrategies
                     .Select(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>(), symbolType)));

        public bool AreAnyTrue(Type type, SymbolType symbolType)
            => AreAnyTrue(attributeStrategies
                     .Select(s => s(type.GetCustomAttributes().OfType<Attribute>(),symbolType)));

        private static bool AreAnyTrue(IEnumerable<bool?> values) 
            => values
                            .Where(x => x.GetValueOrDefault())
                            .Any();

        public bool? AreAnyTrueNullIfNone(ParameterInfo parameterInfo, SymbolType symbolType)
            => AreAnyTrueNullIfNone(attributeStrategies
                    .Select(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>(), symbolType)));

        public bool? AreAnyTrueNullIfNone(PropertyInfo propertyInfo, SymbolType symbolType)
            => AreAnyTrueNullIfNone(attributeStrategies
                     .Select(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>(), symbolType)));

        public bool? AreAnyTrueNullIfNone(Type type, SymbolType symbolType)
            => AreAnyTrueNullIfNone(attributeStrategies
                     .Select(s => s(type.GetCustomAttributes().OfType<Attribute>(), symbolType)));


        private static bool? AreAnyTrueNullIfNone(IEnumerable<bool?> values)
            => values.Any(v => v.HasValue)
                        ? (bool?)values.Where(v => v.HasValue).Any(v => v.Value)
                        : null;
    }
}