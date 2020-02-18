using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.CommandLine.ReflectionModel
{
    public abstract class StrategyBase
    {
        protected StrategyBase(SymbolType symbolType)
            => SymbolType = symbolType;
        public SymbolType SymbolType { get; }
        public abstract string Description { get; }
    }

    public class AttributeStrategy<T> : StrategyBase
    {
        public AttributeStrategy(Expression<Func<Attribute, T>> extract,
                                 Type attributeType,
                                 SymbolType symbolType)
            : base(symbolType)
        {
            AttributeType = attributeType;
            Extract = extract;
            ExtractFunc = extract?.Compile();
        }

        public Type AttributeType { get; }
        public Expression<Func<Attribute, T>> Extract { get; }
        public Func<Attribute, T> ExtractFunc { get; }

        public bool AttributeIsCorrectType(Attribute attribute)
            => AttributeType.IsAssignableFrom(attribute.GetType());

        public T GetValue(Attribute a)
            => ExtractFunc is null
                ? default
                : ExtractFunc(a);

        public override string Description 
            => $"Attribute Strategy: {AttributeType.Name} {Extract.Description()}";
    }

    public static class MoreExtensions
    {
        public static string Description<T>(this Expression<Func<Attribute, T>> expression )
        {
            return "<WIP>";
        }
    }

    // TODO for SourceGeneration: Redesign this class so the same class works for source generation
    public class AttributeStrategies<T>
    {
        private readonly List<AttributeStrategy<T>> attributeStrategies = new List<AttributeStrategy<T>>();

        protected IEnumerable<AttributeStrategy<T>> Strategies
            => attributeStrategies;

        public void AddInternal(AttributeStrategy<T> strategy)
                            => attributeStrategies.Add((strategy));
        public void Add<TAttribute>(Expression<Func<Attribute, T>> extractFunc, SymbolType symbolType = SymbolType.All)
              where TAttribute : Attribute
              => AddInternal(new AttributeStrategy<T>(extractFunc, typeof(TAttribute), symbolType));

        // If multiple attribute types are present, they can disagree. First wins, and it is indeterminate which that is.
        public (bool found, T value) GetFirstValue(IEnumerable<Attribute> attributes, SymbolType symbolType)
        {
            var strategies = Strategies
                              .Where(s => (s.SymbolType & symbolType) != 0);
            var values = strategies
                              .Select(s => attributes
                                            .OfType<Attribute>()
                                            .Where(a => s.AttributeIsCorrectType(a))
                                            .Select(a => s.GetValue(a))
                                            .FirstOrDefault())
                              .Where(x => !EqualityComparer<T>.Default.Equals(x, default));
            return values.Any()
                    ? (true, values.First())
                    : (false, default(T));
        }
    }
}