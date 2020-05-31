using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArgumentDescriptor : SymbolDescriptor
    {
        public ArgumentDescriptor(Type argumentType, ISymbolDescriptor parentSymbolDescriptorBase,
                                   object raw)
            : base(parentSymbolDescriptorBase, raw, SymbolType.Argument)
        {
            ArgumentType = argumentType;
        }

        public ArityDescriptor? Arity { get; set; }
        //TODO: AllowedValues aren't supported in DescriptorMakerBase or tests
        public HashSet<string>? AllowedValues { get; } = new HashSet<string>();
        // TODO: Consider how ArgumentType works when coming from JSON. 
        public Type ArgumentType { get; }
        public DefaultValueDescriptor? DefaultValue { get; set; }
        public bool Required { get; set; }

    }
}
