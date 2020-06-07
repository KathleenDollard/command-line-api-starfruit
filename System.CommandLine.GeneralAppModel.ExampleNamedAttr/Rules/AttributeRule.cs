using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;

namespace System.CommandLine.NamedAttributeRules
{
    /// <summary>
    /// This rule allows either the presence of the attribute or explicitly setting a property to indicate true.
    /// By implication this only works if the attribute's default is false, and it is never logical to use the 
    /// attribute to clarify the default. Otherwise, use NamedAttributeWithPropertyRule
    /// </summary>
    public class AttributeRule : RuleBase, IRuleGetCandidates
    {
        public AttributeRule(string attributeName, SymbolType symbolType = SymbolType.All)
            : base(symbolType)
        {
            AttributeName = attributeName;
        }
        public string AttributeName { get; }

        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> candidates,
                                                    ISymbolDescriptor parentSymbolDescriptor) 
            => candidates.Where(c => GetMatches(parentSymbolDescriptor, c.Traits, parentSymbolDescriptor).Any());

        protected IEnumerable<TTraitType> GetMatches<TTraitType>(ISymbolDescriptor symbolDescriptor,
                                              IEnumerable<TTraitType> traits,
                                              ISymbolDescriptor parentSymbolDescriptor)
            => traits.Where(
                 trait => SpecificSource.Tools.DoesTraitMatch(AttributeName, symbolDescriptor, trait, parentSymbolDescriptor));

        public override string RuleDescription<TIRuleSet>()
           => (typeof(IRuleGetValue<string>).IsAssignableFrom(typeof(TIRuleSet))
                ? "If " : "")
            + $"there is an attribute named '{AttributeName}'";
    }
}
