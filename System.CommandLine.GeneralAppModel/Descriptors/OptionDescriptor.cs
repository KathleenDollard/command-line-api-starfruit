using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class OptionDescriptor : SymbolDescriptorBase
    {
        public OptionDescriptor(SymbolDescriptorBase parentSymbolDescriptorBase,
                                object raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Option) { }

        public IEnumerable<ArgumentDescriptor> Arguments { get; set; }
        public bool Required { get; set; }

        public IEnumerable<RuleBase> AppliedRequiredRules { get; } = new List<RuleBase>();
        // TODO: Create OptionArgumentRules, probably different than CommandArgumentRules: public IEnumerable<RuleBase> AppliedRequiredRules { get; } = new List<RuleBase>();

    }
}
