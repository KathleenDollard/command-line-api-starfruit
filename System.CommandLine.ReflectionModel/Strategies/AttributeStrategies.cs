using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.CommandLine;
using System.CommandLine.GeneralAppModel;

namespace System.CommandLine.ReflectionModel.Strategies
{
    public abstract class StrategyBase
    {
        protected StrategyBase(SymbolType symbolType)
            => SymbolType = symbolType;
        public SymbolType SymbolType { get; }
        public abstract string StrategyDescription { get; }
    }

    public class AttributeStrategy<T> : StrategyBase
    {
        public AttributeStrategy(Expression<Func<Attribute, T>> extract,
                                 Func<Attribute, bool> foundFunc,
                                 Type attributeType,
                                 SymbolType symbolType)
            : base(symbolType)
        {
            AttributeType = attributeType;
            Extract = extract;
            ExtractFunc = extract?.Compile();
            FoundFunc = foundFunc;
        }

        public Type AttributeType { get; }
        public Expression<Func<Attribute, T>> Extract { get; }
        public Func<Attribute, T> ExtractFunc { get; }

        public Func<Attribute, bool> FoundFunc { get; }

        public bool AttributeIsCorrectType(Attribute attribute)
            => AttributeType.IsAssignableFrom(attribute.GetType());

        public (bool found, T value) GetValue(Attribute a)
        {
            var found = FoundFunc is null
                        ? true
                        : FoundFunc(a);
            return ExtractFunc is null
                           ? (false, default)
                           : (found, ExtractFunc(a));
        }

        public override string StrategyDescription
            => $"Attribute: [{AttributeType.Name}] {Extract.Description()}";
    }


    // TODO for SourceGeneration: Redesign this class so the same class works for source generation
    public class AttributeStrategies<T> : RuleStrategies
    {
        private readonly List<AttributeStrategy<T>> attributeStrategies = new List<AttributeStrategy<T>>();

        protected IEnumerable<AttributeStrategy<T>> Strategies
            => attributeStrategies;

        protected void AddInternal(AttributeStrategy<T> strategy)
                            => attributeStrategies.Add((strategy));

        public void Add<TAttribute>(Expression<Func<Attribute, T>> extractFunc, SymbolType symbolType = SymbolType.All)
              where TAttribute : Attribute
              => AddInternal(new AttributeStrategy<T>(extractFunc, null, typeof(TAttribute), symbolType));

        public void Add<TAttribute>(Expression<Func<Attribute, T>> extractFunc, Func<Attribute, bool> foundFunc, SymbolType symbolType = SymbolType.All)
            where TAttribute : Attribute
            => AddInternal(new AttributeStrategy<T>(extractFunc, foundFunc, typeof(TAttribute), symbolType));

        public IEnumerable<T> GetValues(ICustomAttributeProvider customAttributeProvider, SymbolType symbolType)
                => GetValues(customAttributeProvider.GetCustomAttributes(false).OfType<Attribute>(), symbolType);

        public IEnumerable<T> GetValues(IEnumerable<Attribute> attributes, SymbolType symbolType)
          => Strategies
                  .Where(s => (s.SymbolType & symbolType) != 0)
                  .SelectMany(s => attributes.Where(a => s.AttributeIsCorrectType(a))
                                   .Select(a => s.GetValue(a))
                                   .Where(v => v.found)
                                   .Select(v => v.value));


        public (bool found, T value) GetFirstValue(IEnumerable<Attribute> attributes, SymbolType symbolType)
        {
            var values = GetValues(attributes, symbolType);
            return values.Any()
                    ? (true, values.First())
                    : (false, default(T));
        }

        public override IEnumerable<string> StrategyDescriptions
             => attributeStrategies.Select(s => s.StrategyDescription);
    }
}