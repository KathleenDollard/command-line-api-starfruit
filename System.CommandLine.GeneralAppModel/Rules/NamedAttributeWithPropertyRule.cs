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
            var values = GetAllValues(symbolDescriptor, item, parentSymbolDescriptor);
            values = values.Where(x => !Equals(x, default));
            return values.Any()
                    ? (true, values.FirstOrDefault())
                    : (false, default);
            //var values = attributes
            //    .Select(x => SpecificSource.Tools.GetAttributeProperty<TValue>(attributes.FirstOrDefault(), PropertyName));
            //return attributes.Any()
            //    ? (true, values.Where(x=>!x.Equals(default(TValue))).FirstOrDefault())
            //    : ((bool success, TValue value))(false, default);
        }

        public IEnumerable<TValue> GetAllValues(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> item, SymbolDescriptorBase parentSymbolDescriptor)
            => GetMatches(symbolDescriptor, item, parentSymbolDescriptor)
                .SelectMany(a => SpecificSource.Tools.GetAttributeProperties<TValue>(a, PropertyName));


        public override string RuleDescription<TIRuleSet>()
            => $"If there is an attribute named '{AttributeName}', its '{PropertyName}' property, with type {typeof(TValue)}";

    }
}
