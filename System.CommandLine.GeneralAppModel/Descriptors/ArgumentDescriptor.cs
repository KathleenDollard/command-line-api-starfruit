using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArgumentDescriptor : SymbolDescriptor
    {
        public ArgumentDescriptor(ArgTypeInfo argumentTypeInfo, ISymbolDescriptor parentSymbolDescriptorBase,
                                   string originalName,
                                   object? raw)
            : base(parentSymbolDescriptorBase, originalName,raw,  SymbolType.Argument)
        {
            ArgumentType = argumentTypeInfo;
        }

        public ArityDescriptor? Arity { get; set; }
        //TODO: AllowedValues aren't yet supported in DescriptorMakerBase or tests
        public List<object> AllowedValues { get; } = new List<object>();
        // TODO: Consider how ArgumentType works when coming from JSON. If we do Json
        public ArgTypeInfo ArgumentType { get; }
        public DefaultValueDescriptor? DefaultValue { get; set; }
        public bool Required { get; set; }

        public override string ReportInternal(int tabsCount, VerbosityLevel verbosity)
        {
            string whitespace = CoreExtensions.NewLineWithTabs(tabsCount);
            return $"{whitespace}Arity:{Arity}" +
                   $"{whitespace}AllowedValues:{Name}" +
                   $"{whitespace}ArgumentType:{ArgumentType}" +
                   $"{whitespace}DefaultValue:{DefaultValue}" +
                   $"{whitespace}Required:{Required}";
        }
    }
}
