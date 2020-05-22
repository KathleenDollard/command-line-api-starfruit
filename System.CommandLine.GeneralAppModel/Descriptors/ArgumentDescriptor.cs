using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArgumentDescriptor : SymbolDescriptorBase
    {
        public ArgumentDescriptor(SymbolDescriptorBase parentSymbolDescriptorBase,
                                   object raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Argument ) { }

        public ArityDescriptor Arity { get; set; }
        //TODO: AllowedValues aren't supported in DescriptorMakerBase or tests
        public HashSet<string> AllowedValues { get; } = new HashSet<string>();
        // TODO: Consider how ArgumentType works when coming from JSON. 
        public Type ArgumentType { get; set; }
        public DefaultValueDescriptor DefaultValue { get; set; }
        public bool Required { get; set; }

        public IEnumerable<RuleBase> AppliedArityRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedAllowedValuesRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedDefaultValueRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedRequiredRules { get; } = new List<RuleBase>();

    }
}
