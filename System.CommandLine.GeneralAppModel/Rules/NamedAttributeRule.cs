using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Rules;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This rule allows either the presence of the attribute or explicitly setting a property to indicate true.
    /// By implication this only works if the attribute's default is false, and it is never logical to use the 
    /// attribute to clarify the default. Otherwise, use NamedAttributeWithPropertyRule
    /// </summary>
    public class NamedAttributeRule : AttributeRuleBase, IRuleGetCandidates
    {
        public NamedAttributeRule(string attributeName, SymbolType symbolType = SymbolType.All)
            : base(attributeName, symbolType)
        { }

        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> candidates,
                                                    SymbolDescriptorBase parentSymbolDescriptor)
        {
            return candidates
                             .Where(x => x.Traits
                                         .Where(x => SpecificSource.Tools
                                                         .DoesAttributeMatch(AttributeName, x))
                                         .Any());
        }

        public virtual (bool success, bool value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                 IEnumerable<object> items,
                                                                 SymbolDescriptorBase parentSymbolDescriptor)
        {
            var attributes = GetMatches(symbolDescriptor, items, parentSymbolDescriptor);

            return attributes.Any()
                    ? (true, true)
                    : (false, false);
        }

        public override string RuleDescription<TIRuleSet>()
           => (typeof(IRuleGetValue<string>).IsAssignableFrom(typeof(TIRuleSet))
                ? "If " : "")
            + $"there is an attribute named '{AttributeName}'";
    }
}
