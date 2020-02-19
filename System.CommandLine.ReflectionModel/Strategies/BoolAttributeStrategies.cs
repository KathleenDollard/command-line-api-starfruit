using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{
    public class BoolAttributeStrategies : AttributeStrategies<bool?>
    {
        public void Add<TAttribute>(SymbolType symbolType = SymbolType.All)
                 where TAttribute : Attribute
                        => AddInternal(new AttributeStrategy<bool?>(a => true, null, typeof(TAttribute), symbolType));

        // Based on the type passed - which in the future could be a Roslyn class or JSON fragment, determine values, then call logic
        public bool AreAnyTrue(ICustomAttributeProvider customAttributeProvider, SymbolType symbolType) 
            => AreAnyTrue(GetValues(customAttributeProvider, symbolType));

        public bool? AreAnyTrueNullIfNone(ICustomAttributeProvider customAttributeProvider, SymbolType symbolType) 
            => AreAnyTrueNullIfNone(GetValues(customAttributeProvider, symbolType));

        private static bool? AreAnyTrueNullIfNone(IEnumerable<bool?> values)
            => values.Where(x => x.HasValue)
                      .Any()
                          ? values.Any(x => x.Value)
                          : (bool?)null;

        private static bool AreAnyTrue(IEnumerable<bool?> values)
            => values.Any(x => x.GetValueOrDefault());
    
    }
}