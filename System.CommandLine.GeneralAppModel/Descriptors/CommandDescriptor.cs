using System.Collections.Generic;
using System.CommandLine.Binding;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class CommandDescriptor : SymbolDescriptor
    {
        public CommandDescriptor(ISymbolDescriptor parentSymbolDescriptorBase,
                                 object? raw)
            : base(parentSymbolDescriptorBase, raw,  SymbolType.Command) { }

        public bool TreatUnmatchedTokensAsErrors { get; set; } = true;
        public List<ArgumentDescriptor> Arguments { get; } = new List<ArgumentDescriptor>();
        public List<OptionDescriptor> Options { get; } = new List<OptionDescriptor>();
        public InvokeMethodInfo? InvokeMethod { get; set; } // in Reflection models, this is a MethodInfo, in Roslyn it will be something else
        public List<CommandDescriptor> SubCommands { get; } = new List<CommandDescriptor>();

        public override string ReportInternal(int tabsCount, VerbosityLevel verbosity )
        {
            string whitespace = CoreExtensions.NewLineWithTabs(tabsCount);
            return $"{whitespace}TreatUnmatchedTokensAsErrors:{TreatUnmatchedTokensAsErrors}" +
                   $"{whitespace}SubCommands:{string.Join("", SubCommands.Select(x => x.Report(tabsCount + 1, verbosity)))}" +
                   $"{whitespace}Options:{string.Join("", Options.Select(x => x.Report(tabsCount + 1, verbosity)))}" +
                   $"{whitespace}Arguments:{string.Join("", Arguments.Select(x => x.Report(tabsCount + 1, verbosity)))}";
        }
    }
}
