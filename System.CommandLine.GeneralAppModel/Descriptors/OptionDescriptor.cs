using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class OptionDescriptor : SymbolDescriptorBase
    {
        public OptionDescriptor(SymbolDescriptorBase parentSymbolDescriptorBase,
                                object raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Option) { }

        public List<ArgumentDescriptor> Arguments { get; } = new List<ArgumentDescriptor>();
        public bool Required { get; set; }

    }
}
