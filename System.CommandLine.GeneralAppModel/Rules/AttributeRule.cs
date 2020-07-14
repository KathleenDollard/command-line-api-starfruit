using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This rule allows either the presence of the attribute or explicitly setting a property to indicate true.
    /// By implication this only works if the attribute's default is false, and it is never logical to use the 
    /// attribute to clarify the default. Otherwise, use NamedAttributeWithPropertyRule
    /// </summary>
    public class AttributeRule<TAttribute> : RuleBase, IRuleGetCandidates
    {
        public AttributeRule(SymbolType symbolType = SymbolType.All)
            : base(symbolType)
        {
        }

        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> candidates,
                                                    ISymbolDescriptor parentSymbolDescriptor)
            => candidates.Where(c => GetMatches(parentSymbolDescriptor, c.Traits, parentSymbolDescriptor).Any());

        protected IEnumerable<TTraitType> GetMatches<TTraitType>(ISymbolDescriptor symbolDescriptor,
                                              IEnumerable<TTraitType> traits,
                                              ISymbolDescriptor parentSymbolDescriptor)
            => traits.Where(
                 trait => DescriptorMakerSpecificSourceBase.Tools.DoesTraitMatch<TAttribute, TTraitType>(symbolDescriptor, trait, parentSymbolDescriptor));

        public override string RuleDescription<TIRuleSet>()
           =>  $"with an attribute named '{typeof(TAttribute).Name.WithoutAttributeSuffix()}'";
    }
}
