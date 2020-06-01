using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This rule returns candidates for all types derived from the parent description type. 
    /// </summary>
    public class DerivedFromRule : RuleBase, IRuleGetAvailableCandidates, IAbstractRule
    {
        public DerivedFromRule(string? assemblyName = null, string? namespaceName = null, bool ignoreNamespace = false)
            : base(SymbolType.Command)
        {
            NamespaceName = namespaceName;
            AssemblyName = assemblyName;
            IgnoreNamespace = ignoreNamespace;
        }

        public string? NamespaceName { get; }
        public string? AssemblyName { get; }
        public bool IgnoreNamespace { get; }

        public virtual IEnumerable<Candidate> GetChildCandidates(ISymbolDescriptor generalSymbolDescriptor)
        {
            return new List<Candidate>();
        }

        public override string RuleDescription<TIRuleSet>()
            => $"DerivedFrom Abstract Rule: NamespaceName: {NamespaceName} AssemblyName: {AssemblyName} IgnoreNamespace: {IgnoreNamespace}";

    }
}

