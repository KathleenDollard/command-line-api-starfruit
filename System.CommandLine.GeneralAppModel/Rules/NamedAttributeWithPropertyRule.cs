using System.Collections;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Rules;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class NamedAttributeWithPropertyRule : AttributeRuleBase
    {
        public NamedAttributeWithPropertyRule(string attributeName, string propertyName, Type type, SymbolType symbolType = SymbolType.All)
        : base(attributeName, symbolType)
        {
            PropertyName = propertyName;
            Type = type;
        }

        public string PropertyName { get; }
        public Type Type { get; }

        //private IEnumerable<Attribute> GetMatchingAttributes(SymbolDescriptorBase symbolDescriptor, IEnumerable<Attribute> items)
        //{
        //    return SymbolType != SymbolType.All && SymbolType != symbolDescriptor.SymbolType
        //        ? Array.Empty<Attribute>()
        //        : items
        //            .Where(a => SpecificSource.Tools.DoesAttributeMatch(AttributeName, a));
        //}

        //private IEnumerable<Attribute> GetAttributes(ICustomAttributeProvider attributeProvider)
        //    => attributeProvider.GetCustomAttributes(Context.IncludeBaseClassAttributes)
        //                                .OfType<Attribute>();

    }

    public class NamedAttributeWithPropertyRule<TValue> : NamedAttributeWithPropertyRule, IRuleGetValue<TValue>, IRuleGetValues<TValue>
    {
        public NamedAttributeWithPropertyRule(string attributeName, string propertyName, SymbolType symbolType = SymbolType.All)
        : base(attributeName, propertyName, typeof(TValue), symbolType)
        { }

        public (bool success, TValue value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                   IEnumerable<object> item,
                                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var attributes = GetMatches(symbolDescriptor, item, parentSymbolDescriptor);
            if (attributes.Any())
            {
                return (true, SpecificSource.Tools.GetAttributeProperty<TValue>(attributes.FirstOrDefault(), PropertyName));
            }
            return (false, default);
        }

        public IEnumerable<TValue> GetAllValues(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> item, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var attributes = GetMatches(symbolDescriptor, item, parentSymbolDescriptor);
            return attributes.SelectMany(a => SpecificSource.Tools. GetAttributeProperties<TValue>(a, PropertyName));
        }

        //private static T GetProperty<T>(Attribute attribute, string propertyName)
        //{
        //    var raw = attribute.GetType()
        //                      .GetProperty(propertyName)
        //                      .GetValue(attribute);
        //    return Conversions.To<T>(raw);
        //}

        //private static IEnumerable<T> GetProperties<T>(Attribute attribute, string propertyName)
        //{
        //    var property = attribute.GetType()
        //                      .GetProperty(propertyName);
        //    var raw = property.GetValue(attribute);
        //    if (typeof(T).IsAssignableFrom(property.PropertyType))
        //    {
        //        return new T[] { Conversions.To<T>(raw) };
        //    }
        //    if (typeof(IEnumerable<T>).IsAssignableFrom(property.PropertyType) && raw is IEnumerable<T> x)
        //    {
        //        return x.Select(xx => Conversions.To<T>(xx));
        //    }
        //    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && raw is IEnumerable y)
        //    {
        //        var list = new List<T>();
        //        foreach (var item in y)
        //        {
        //            list.Add(Conversions.To<T>(item));
        //        }
        //        return list;
        //    }
        //    throw new InvalidOperationException("Unhandled Attribute PropertyType");
        //}

        //private IEnumerable<Attribute> GetMatchingAttributes(SymbolDescriptorBase symbolDescriptor, IEnumerable<Attribute> items)
        //{
        //    return SymbolType != SymbolType.All && SymbolType != symbolDescriptor.SymbolType
        //        ? Array.Empty<Attribute>()
        //        : items
        //            .Where(a => SpecificSource.Tools.DoesAttributeMatch(AttributeName, a));
        //}

        //private IEnumerable<Attribute> GetAttributes(ICustomAttributeProvider attributeProvider)
        //    => attributeProvider.GetCustomAttributes(Context.IncludeBaseClassAttributes)
        //                                .OfType<Attribute>();

        public override string RuleDescription<TIRuleSet>()
            => $"If there is an attribute named '{AttributeName}', its '{PropertyName}' property, with type {typeof(TValue)}";

    }
}
