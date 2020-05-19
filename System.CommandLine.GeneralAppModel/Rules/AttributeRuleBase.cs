using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Rules
{
    public abstract class AttributeRuleBase : RuleBase
    {
        public AttributeRuleBase(string attributeName, SymbolType symbolType = SymbolType.All)
            : base(symbolType)
        {
            AttributeName = attributeName;
        }

        public string AttributeName { get; }

        protected IEnumerable<T> GetMatches<T>(SymbolDescriptorBase symbolDescriptor,
                                             IEnumerable<T> items,
                                             SymbolDescriptorBase parentSymbolDescriptor)
           => items
                   .Where(item => SpecificSource.Tools.IsAttributeAMatch(AttributeName, symbolDescriptor, item,
                                                                         parentSymbolDescriptor));

        //protected bool IsMatch(SymbolDescriptorBase symbolDescriptor,
        //              object item,
        //              SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    return item switch
        //    {
        //        Attribute a => DoesAttributeMatch(AttributeName, a),
        //        _ => false
        //    };
        //}

        //protected bool DoesAttributeMatch(string attributeName, Attribute a)
        //{
        //    var itemName = a.GetType().Name;
        //    return itemName.Equals(attributeName, StringComparison.OrdinalIgnoreCase)
        //        || itemName.Equals(attributeName + "Attribute", StringComparison.OrdinalIgnoreCase);
        //}
    }
}
