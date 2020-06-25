using System.Collections.Generic;
using System.CommandLine.Binding;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class CommandDescriptor : SymbolDescriptor
    {
        public CommandDescriptor(ISymbolDescriptor parentSymbolDescriptorBase,
                                 object? raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Command) { }

        public bool TreatUnmatchedTokensAsErrors { get; set; } = true;
        public List<ArgumentDescriptor> Arguments { get; } = new List<ArgumentDescriptor>();
        public List<OptionDescriptor> Options { get; } = new List<OptionDescriptor>();
        public InvokeMethodInfo? InvokeMethod { get; set; } // in Reflection models, this is a MethodInfo, in Roslyn it will be something else
        public List<CommandDescriptor> SubCommands { get; } = new List<CommandDescriptor>();

    }
}
