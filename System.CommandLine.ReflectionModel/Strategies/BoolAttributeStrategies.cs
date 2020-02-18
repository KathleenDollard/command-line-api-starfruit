using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{

    public class BoolAttributeStrategies : AttributeStrategies<bool?>
    {
        // For each attribute, a single value is returned. Unless an attribute can be placed multiple times, this will be single. 
        // If multiple attribute types are present, they can disagree. True wins.

        public void Add<TAttribute>(SymbolType symbolType = SymbolType.All)
                 where TAttribute : Attribute
                        => AddInternal(new AttributeStrategy<bool?>(a => true, typeof(TAttribute), symbolType));


        public bool AreAnyTrue(ParameterInfo parameterInfo, SymbolType symbolType)
            => AreAnyTrue(parameterInfo.GetCustomAttributes(), symbolType);

        public bool AreAnyTrue(PropertyInfo propertyInfo, SymbolType symbolType)
            => AreAnyTrue(propertyInfo.GetCustomAttributes(), symbolType);

        public bool AreAnyTrue(Type type, SymbolType symbolType)
            => AreAnyTrue(type.GetCustomAttributes(), symbolType);

        public bool? AreAnyTrueNullIfNone(ParameterInfo parameterInfo, SymbolType symbolType)
            => AreAnyTrueNullIfNone(parameterInfo.GetCustomAttributes(), symbolType);

        public bool? AreAnyTrueNullIfNone(PropertyInfo propertyInfo, SymbolType symbolType)
            => AreAnyTrueNullIfNone(propertyInfo.GetCustomAttributes(), symbolType);

        public bool? AreAnyTrueNullIfNone(Type type, SymbolType symbolType)
            => AreAnyTrueNullIfNone(type.GetCustomAttributes(), symbolType);

        private bool AreAnyTrue(IEnumerable<Attribute> attributes, SymbolType symbolType)
        {
            IEnumerable<AttributeStrategy<bool?>> strategies = Strategies
                    .Where(s => (s.SymbolType & symbolType) != 0);
            return strategies
                      .Any(s => attributes
                                            .OfType<Attribute>()
                                            .Where(a => s.AttributeIsCorrectType(a))
                                            .Any(a => s.GetValue(a).GetValueOrDefault()));
        }

        private bool? AreAnyTrueNullIfNone(IEnumerable<Attribute> attributes, SymbolType symbolType)
        {
            var matchingAttributes = Strategies
                  .Where(s => (s.SymbolType & symbolType) != 0)
                  .Select(s => attributes
                               .OfType<Attribute>()
                               .Where(a => s.AttributeIsCorrectType(a))
                               .Select(a => s.GetValue(a)))
                  .SelectMany(x => x)
                  .Where(x => x.HasValue);
            return matchingAttributes.Any()
                   ? matchingAttributes.Any(x => x.GetValueOrDefault())
                   : (bool?)null;
        }

    }
}