using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace System.CommandLine.ReflectionModel
{
    public class AttributeStrategy<T>
    {
        // TODO: Extend strategy replacement for Func to other strategy types
        // TODO: Should all strategies have a symbol type filter
        public AttributeStrategy(Type attributeType,
                                 Expression<Func<Attribute, T>> extract)
        {
            AttributeType = attributeType;
            Extract = extract;
            ExtractFunc = extract.Compile();
        }

        public Type AttributeType { get; }
        public Expression<Func<Attribute, T>> Extract { get; }
        public Func<Attribute, T> ExtractFunc { get; }

        public bool AttributeIsCorrectType(Attribute attribute)
            => AttributeType.IsAssignableFrom(attribute.GetType());

        public T Check(Attribute a)
            => ExtractFunc(a);
    }

    // TODO for SourceGeneration: Redesign this class so the same class works for source generation
    public class AttributeStrategies2<T>
    {
        protected readonly List<AttributeStrategy<T>> attributeStrategies = new List<AttributeStrategy<T>>();
        public void AddInternal(AttributeStrategy<T> strategy)
                            => attributeStrategies.Add((strategy));
    }

    public class BoolAttributeStrategies2 : AttributeStrategies2<bool?>
    {
        // For each attribute, a single value is returned. Unless an attribute can be placed multiple times, this will be single. 
        // If multiple attribute types are present, they can disagree. True wins.
        public void Add<T>(Expression<Func<Attribute, bool?>> extractFunc)
            where T : Attribute
            => AddInternal(new AttributeStrategy<bool?>(typeof(T), extractFunc));

        public void Add<T>()
             where T : Attribute
            => AddInternal(new AttributeStrategy<bool?>(typeof(T), null));

        public bool AreAnyTrue(ParameterInfo parameterInfo)
            => AreAnyTrue(parameterInfo.GetCustomAttributes());

        public bool AreAnyTrue(PropertyInfo propertyInfo)
            => AreAnyTrue(propertyInfo.GetCustomAttributes());

        public bool AreAnyTrue(Type type)
            => AreAnyTrue(type.GetCustomAttributes());

        public bool? AreAnyTrueNullIfNone(ParameterInfo parameterInfo)
            => AreAnyTrueNullIfNone(parameterInfo.GetCustomAttributes());

        public bool? AreAnyTrueNullIfNone(PropertyInfo propertyInfo)
            => AreAnyTrueNullIfNone(propertyInfo.GetCustomAttributes());

        public bool? AreAnyTrueNullIfNone(Type type)
            => AreAnyTrueNullIfNone(type.GetCustomAttributes());

        private bool AreAnyTrue(IEnumerable<Attribute> attributes)
             => attributeStrategies
                     .Any(s => attributes
                                 .OfType<Attribute>()
                                 .Where(a => s.AttributeIsCorrectType(a))
                                 .Any(a => s.Check(a).GetValueOrDefault()));

        private bool? AreAnyTrueNullIfNone(IEnumerable<Attribute> attributes)
        {
            var matchingAttributes = attributeStrategies
                  .Select(s => attributes
                               .OfType<Attribute>()
                               .Where(a => s.AttributeIsCorrectType(a))
                               .Select(a => s.Check(a)))
                  .SelectMany(x => x)
                  .Where(x => x.HasValue);
            return matchingAttributes.Any()
                   ? matchingAttributes.Any(x => x.GetValueOrDefault())
                   : (bool?)null;
        }

    }
}