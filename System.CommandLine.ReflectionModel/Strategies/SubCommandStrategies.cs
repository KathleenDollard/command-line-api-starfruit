using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.CommandLine.ReflectionModel
{
    public class SubCommandStrategies
    {
        private readonly List<Func<Type, TypeCache, IEnumerable<Type>>> typeStrategies = new List<Func<Type, TypeCache, IEnumerable<Type>>>();
        private TypeCache typeCache;

        public IEnumerable<Type> GetCommandTypes(Type type)
        {
            if (typeCache is null || type.Assembly != typeCache.Assembly)
            {
                typeCache = new TypeCache(type);
            }
            return typeStrategies.SelectMany(s => s(type, typeCache));
        }

        public void AddTypeStrategy(Func<Type, TypeCache, IEnumerable<Type>> strategy)
            => typeStrategies.Add(strategy);
    }

    public class TypeCache
    {
        public TypeCache(Type sampleType)
            => Assembly = sampleType.Assembly;

        public Assembly Assembly { get; }

        public IEnumerable<Type> GetDerivedTypes(Type baseType)
        {
            if (Types.ContainsKey(baseType))
            {
                return Types[baseType];
            }
            return new List<Type>();
        }

        private Dictionary<Type, List<Type>> _types;

        private Dictionary<Type, List<Type>> Types
        {
            get
            {
                if (_types == null)
                {
                    _types = new Dictionary<Type, List<Type>>();
                    Type[] ts = Assembly.GetExportedTypes();
                    foreach (Type t in ts)
                    {
                        if (t.BaseType != null && t.BaseType != typeof(object))
                        {
                            if (_types.ContainsKey(t.BaseType))
                            {
                                _types[t.BaseType].Add(t);
                                continue;
                            }
                            _types.Add(t.BaseType, new List<Type>() { t });
                        }
                    }
                }
                return _types;
            }
        }
    }

    public static class SubCommandStrategiesExtensions
    {
        public static SubCommandStrategies AllStandard(this SubCommandStrategies subCommandStrategies)
            => subCommandStrategies.DerivedTypes();

        public static SubCommandStrategies DerivedTypes(this SubCommandStrategies subCommandStrategies)
        {

            subCommandStrategies.AddTypeStrategy((type, typeCache) => typeCache.GetDerivedTypes(type));
            return subCommandStrategies;
        }
    }
}
