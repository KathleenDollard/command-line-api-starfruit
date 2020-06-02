using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace System.CommandLine.ReflectionAppModel
{
    /// <summary>
    /// This rule returns candidates for all types derived from the parent description type. 
    /// This rule assumes that all candidates will be in the same assembly and in the same
    /// namespace unless ignore namespace is set to true.
    /// </summary>
    public class DerivedFromReflectionRule : DerivedFromRule
    {
        private IEnumerable<Type>? cacheTypes;
        private string? cacheAssemblyName;
        private string? cacheNamespaceName;

        public DerivedFromReflectionRule(string? assemblyName = null, string? namespaceName = null, bool ignoreNamespace = false)
        {
            useTypeAssembly = AssemblyName is null;
        }
        private bool useTypeAssembly { get; set; }

        public override string RuleDescription<TIRuleSet>()
            => $"DerivedFromReflectionRule Rule: NamespaceName: {NamespaceName} AssemblyName: {AssemblyName} IgnoreNamespace: {IgnoreNamespace}";

        public override IEnumerable<Candidate> GetChildCandidates(ISymbolDescriptor generalSymbolDescriptor)
        {
            if (!(generalSymbolDescriptor is SymbolDescriptor symbolDescriptor)
                    || !(symbolDescriptor.Raw is Type type))
            {
                return new List<Candidate>();
            }

            string namespaceName = (string.IsNullOrEmpty(NamespaceName)
                                        ? type.Namespace
                                        : NamespaceName)!; // The type always has a non-null namespace
            var assemblyName = IgnoreNamespace
                                ? null
                                : string.IsNullOrEmpty(AssemblyName)
                                    ? type.Assembly.GetName().Name
                                    : AssemblyName;
            var assembly = useTypeAssembly
                            ? type.Assembly
                            : GetNamedAssembly(type, assemblyName);
            _ = assembly ?? throw new InvalidOperationException($"Assembly {assemblyName} not found");

            var types = GetTypes(type, namespaceName, assembly);
            return types.Select(t => new Candidate(t));
        }

        private Assembly GetNamedAssembly(Type type, string? assemblyName)
                => assemblyName is null
                    ? type.Assembly
                    : Thread.GetDomain().GetAssemblies().Where(a => a.GetName().Name == assemblyName).FirstOrDefault();

        private IEnumerable<Type> GetTypes(Type type, string namespaceName, Assembly assembly)
        {
            RefreshCacheIfNeeded(namespaceName, assembly);
            return cacheTypes.Where(t => t.BaseType == type);
        }

        private void RefreshCacheIfNeeded(string namespaceName, Assembly assembly)
        {
            if (cacheTypes == null
                || assembly.FullName != cacheAssemblyName
                || (!(namespaceName is null) && namespaceName != cacheNamespaceName))
            {
                cacheTypes = assembly.GetTypes().Where(t => t.GetType() != typeof(object) && (namespaceName is null || t.Namespace == namespaceName));
                cacheAssemblyName = assembly.FullName;
                cacheNamespaceName = namespaceName;
            }
        }
    }
}

