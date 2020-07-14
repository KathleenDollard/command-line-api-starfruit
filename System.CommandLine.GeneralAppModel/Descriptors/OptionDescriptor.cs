using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class OptionDescriptor : SymbolDescriptor
    {
        public OptionDescriptor(ISymbolDescriptor parentSymbolDescriptorBase, 
                                string originalName,
                                object? raw)
            : base(parentSymbolDescriptorBase, originalName, raw, SymbolType.Option) { }

        public List<ArgumentDescriptor> Arguments { get; } = new List<ArgumentDescriptor>();
        public bool Required { get; set; }
        public string? Prefix { get; set; }

        public override string ReportInternal(int tabsCount, VerbosityLevel verbosity)
        {
            string whitespace = CoreExtensions.NewLineWithTabs(tabsCount);
            return $"{whitespace}Required:{Required}";
        }

    }
}
