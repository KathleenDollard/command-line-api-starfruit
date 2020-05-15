using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    public class PropertiesFromTypeRule : RuleBase, IRuleGetAvailableCandidates
    {
        public PropertiesFromTypeRule()
          : base(SymbolType.All)
        { }

        public override string RuleDescription<TIRuleSet>()
                => $"Not yet implemented";

        public IEnumerable<Candidate> GetChildCandidates(SymbolDescriptorBase desc)
        {
            if (!(desc.Raw is Type type))
            {
                return new List<Candidate>();
            }

            return type.GetProperties ().Select(p => new Candidate(p));
        }

    }
}
