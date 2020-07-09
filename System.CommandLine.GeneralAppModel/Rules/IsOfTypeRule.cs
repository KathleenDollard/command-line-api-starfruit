using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class IsOfTypeRule : RuleBase
    {
        public IsOfTypeRule(SymbolType symbolType, Type type)
              : base(symbolType)
        {
            Type = type;
        }

        public Type Type { get; }

        public override string RuleDescription<TIRuleSet>()
            => $"Item is of type {Type.NameWithGenericArguments()}.";
    }

    public class IsOfTypeRule<TType> : IsOfTypeRule, IRuleGetCandidates
    {
        public IsOfTypeRule(SymbolType symbolType)
            : base(symbolType, typeof(TType))
        { }

        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> candidates, ISymbolDescriptor parentSymbolDescriptor)
        {
            if (!(parentSymbolDescriptor is SymbolDescriptor nonEmptySymbolDescriptor))
            {
                return new List<Candidate>();
            }
            return candidates
                  .Where(c => !c.Item.Equals(nonEmptySymbolDescriptor.Raw) && c.Item is TType)
                  .ToList();
        }
    }
}
