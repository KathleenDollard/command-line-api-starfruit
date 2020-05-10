using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArgumentDescriptor : SymbolDescriptorBase
    {
        public ArgumentDescriptor(SymbolDescriptorBase parentSymbolDescriptorBase,
                                   object raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Argument ) { }

        public ArityDescriptor Arity { get; set; }
        public HashSet<string> AllowedValues { get; } = new HashSet<string>();
        public Type ArgumentType { get; set; }
        public DefaultValueDescriptor DefaultValue { get; set; }
        public bool Required { get; set; }

        public IEnumerable<RuleBase> AppliedArityRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedAllowedValuesRules { get; } = new List<RuleBase>();
        // TODO: Consider this: AgumentType is not currently set via a rule. It is set by the AppModel Style. This may not work for JSON
        // public IEnumerable<RuleBase> AppliedArgumentTypeRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedDefaultValueRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedRequiredRules { get; } = new List<RuleBase>();

    }
}
