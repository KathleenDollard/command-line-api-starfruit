using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArgumentDescriptor
    {
        public object Raw { get; set; }
        public IEnumerable<string> Aliases { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHidden { get; set; }
        public ArityDescriptor Arity { get; set; }
        public HashSet<string> AllowedValues { get; } = new HashSet<string>();
        public Type ArgumentType { get; set; }
        public DefaultValueDescriptor DefaultValue { get; set; }
        public bool Required { get; set; }
    }
}
