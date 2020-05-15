using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    public class ParametersFromMethodInfo : RuleBase, IRuleGetAvailableCandidates
    {

        public ParametersFromMethodInfo()
          : base(SymbolType.All)
        { }

        public override string RuleDescription<TIRuleSet>()
                => $"Not yet implemented";

        public IEnumerable<Candidate> GetAvailableCandidates(SymbolDescriptorBase desc)
        {
            if (!(desc.Raw is MethodInfo methodInfo))
            {
                return new List<Candidate>();
            }

            return methodInfo.GetParameters().Select(p => new Candidate(p));
        }

    }
}
