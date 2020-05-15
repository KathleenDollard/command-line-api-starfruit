using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Rules;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This rule allows either the presence of the attribute or explicitly setting a property to indicate true
    /// </summary>
    public class NamedAttributeRule : AttributeRuleBase<bool>
    {
        public NamedAttributeRule(string attributeName,  SymbolType symbolType = SymbolType.All)
            : base(attributeName, symbolType)
        {   }

        public override (bool success, bool value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                 IEnumerable<object> item,
                                                                 SymbolDescriptorBase parentSymbolDescriptor)
        {
            var attributes = GetMatches(symbolDescriptor, item, parentSymbolDescriptor)
                                .OfType<Attribute>();

            return attributes.Any()
                    ? (true, true)
                    : (false, false);
        }

        public override string RuleDescription<TIRuleSet>()
           => $"If there is an attribute named '{AttributeName}'";
    }
}
