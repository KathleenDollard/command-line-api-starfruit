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
    }
}
