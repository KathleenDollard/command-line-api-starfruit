using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class CommandDescriptor : SymbolDescriptorBase
    {
        public CommandDescriptor(SymbolDescriptorBase parentSymbolDescriptorBase,
                                   object raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Command) { }

        public bool TreatUnmatchedTokensAsErrors { get; set; } = true;
        public List<ArgumentDescriptor> Arguments { get; } = new List<ArgumentDescriptor>();
        public List<OptionDescriptor> Options { get; } = new List<OptionDescriptor>();
        public List<CommandDescriptor> SubCommands { get; } = new List<CommandDescriptor>();
        // TODO: Yes! Include it: Set by the model or the configuration: public bool TreatUnmatchedTokensAsErrors { get; set; } = true;

    }
}
