using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class CommandDescriptor : SymbolDescriptor
    {
        public CommandDescriptor(ISymbolDescriptor parentSymbolDescriptorBase,
                                 object? raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Command) { }

        public Symbol SymbolToBind { get; private set; }

        public void SetBinding(Symbol symbol)
            => SymbolToBind = symbol;

        public bool TreatUnmatchedTokensAsErrors { get; set; } = true;
        public List<ArgumentDescriptor> Arguments { get; } = new List<ArgumentDescriptor>();
        public List<OptionDescriptor> Options { get; } = new List<OptionDescriptor>();
        public InvokeMethodInfo? InvokeMethod { get; set; } // in Reflection models, this is a MethodInfo, in Roslyn it will be something else
        public List<CommandDescriptor> SubCommands { get; } = new List<CommandDescriptor>();

    }
}
