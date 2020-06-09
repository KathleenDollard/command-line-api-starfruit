using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Rules
{
    public class NamedInvokeMethodRule : RuleBase, IRuleOptionalValue<InvokeMethodInfo>
    {
        public NamedInvokeMethodRule(string name, bool treatParametersAsCandidates = true)
            : base(SymbolType.Command)
        {
            Name = name;
            TreatParametersAsCandidates = treatParametersAsCandidates;
        }

        public string Name { get; }
        public bool TreatParametersAsCandidates { get; }

        public (bool success, InvokeMethodInfo value) GetOptionalValue(ISymbolDescriptor symbolDescriptor, IEnumerable<object> item, ISymbolDescriptor parentSymbolDescriptor)
        {
            if (symbolDescriptor is CommandDescriptor commandDescriptor)
            {
                var available = SpecificSource.Tools.GetAvailableInvokeMethodInfos(commandDescriptor.Raw, commandDescriptor, TreatParametersAsCandidates );
                var match = available.Where(x => x.Name == Name);
                if (!match.Any())
                {
                    return (false, null);
                }
                var max = match.Max(x => x.Score);
                return match.Count() switch
                {
                    0 => (false, null),
                    1 => (true, match.First()),
                    2 => (true, match.First(x => x.Score == max))
                };
            }
            return (false, null);
        }

        public override string RuleDescription<TIRuleSet>()
                => $"If there is an method named '{Name}', invoke it when the command should run.";
    }
}
