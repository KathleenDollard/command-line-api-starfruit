using System.Collections.Generic;
using System.CommandLine.ReflectionModel.Strategies;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel.ModelStrategies
{
    public class SubCommandStrategies : ModelStrategies
    {
        internal readonly List<TypeStrategy> TypeStrategies = new List<TypeStrategy>();

        public IEnumerable<Type> GetCommandTypes(Type type)
            => TypeStrategies.SelectMany(s => s.GetCommandTypes(type));

        public override IEnumerable<string> StrategyDescriptions
            => TypeStrategies.Select(s => s.StrategyDescription);
    }

    public class TypeStrategy : StrategyBase
    {
        private TypeCache typeCache;

        public TypeStrategy(Func<Type, TypeCache, IEnumerable<Type>> getCommandFunc,
                            SymbolType symbolType = SymbolType.All)
            : base(symbolType)
            => GetCommandFunc = getCommandFunc;

        public Func<Type, TypeCache, IEnumerable<Type>> GetCommandFunc { get; }

        public IEnumerable<Type> GetCommandTypes(Type type)
        {
            if (typeCache is null || type.Assembly != typeCache.Assembly)
            {
                typeCache = new TypeCache(type);
            }
            return GetCommandFunc(type, typeCache);
        }

        public override string StrategyDescription
            => $"Type Strategy: Derived Types";

    }

    public class TypeCache
    {
        public TypeCache(Type sampleType)
            => Assembly = sampleType.Assembly;

        public Assembly Assembly { get; }

        public IEnumerable<Type> GetDerivedTypes(Type baseType)
            => Types.ContainsKey(baseType)
                ? Types[baseType]
                : new List<Type>();

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

            subCommandStrategies.TypeStrategies.Add(new TypeStrategy((type, typeCache) => typeCache.GetDerivedTypes(type)));
            return subCommandStrategies;
        }
    }
}
