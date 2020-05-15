using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Rules
{
    public abstract class AttributeRuleBase<TValue> : RuleBase, IRuleGetValue<TValue>
    {
        public AttributeRuleBase(string attributeName, SymbolType symbolType = SymbolType.All)
            : base(symbolType)
        {
            AttributeName = attributeName;
        }

        public string AttributeName { get; }

        public abstract (bool success, TValue value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                 IEnumerable<object> item,
                                                                 SymbolDescriptorBase parentSymbolDescriptor);


        protected IEnumerable<T> GetMatches<T>(SymbolDescriptorBase symbolDescriptor,
                                             IEnumerable<T> items,
                                             SymbolDescriptorBase parentSymbolDescriptor)
           => items
                   .Where(item => IsMatch(symbolDescriptor, item, parentSymbolDescriptor))
                   .Where(x => !x.Equals(default));

        protected bool IsMatch(SymbolDescriptorBase symbolDescriptor,
                      object item,
                      SymbolDescriptorBase parentSymbolDescriptor)
        {
            return item switch
            {
                Attribute a => DoesAttributeMatch(AttributeName, a),
                _ => false
            };
        }

        protected bool DoesAttributeMatch(string attributeName, Attribute a)
        {
            var itemName = a.GetType().Name;
            return itemName.Equals(attributeName, StringComparison.OrdinalIgnoreCase)
                || itemName.Equals(attributeName + "Attribute", StringComparison.OrdinalIgnoreCase);
        }
    }
}
