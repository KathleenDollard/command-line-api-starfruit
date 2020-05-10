using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class CommandDescriptor : SymbolDescriptorBase
    {
        public CommandDescriptor(SymbolDescriptorBase parentSymbolDescriptorBase,
                                   object raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Command) { }

        public List<ArgumentDescriptor> Arguments { get; } = new List<ArgumentDescriptor>();
        public List<OptionDescriptor> Options { get; } = new List<OptionDescriptor>();
        public List<CommandDescriptor> SubCommands { get; } = new List<CommandDescriptor>();
        // TODO: JON: Set by the model or the configuration: public bool TreatUnmatchedTokensAsErrors { get; set; } = true;

        public IEnumerable<RuleBase> AppliedArgumentRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedOptionRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedSubCommandRules { get; } = new List<RuleBase>();

    }
}
