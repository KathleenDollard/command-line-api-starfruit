using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Rules;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Rules
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
            => $"Item is of type {Type}.";
    }

    public class IsOfTypeRule<TType> : IsOfTypeRule, IRuleGetCandidates
    {
        public IsOfTypeRule(SymbolType symbolType)
            : base(symbolType, typeof(TType))
        { }

        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> candidates, SymbolDescriptorBase parentSymbolDescriptor)
            => candidates.Where(c => c.Item is TType);

    }
}
