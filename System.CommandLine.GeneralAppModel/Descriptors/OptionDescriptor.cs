using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class OptionDescriptor : SymbolDescriptor
    {
        public OptionDescriptor(ISymbolDescriptor parentSymbolDescriptorBase,
                                object? raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Option) { }

        public List<ArgumentDescriptor> Arguments { get; } = new List<ArgumentDescriptor>();
        public bool Required { get; set; }

    }
}
