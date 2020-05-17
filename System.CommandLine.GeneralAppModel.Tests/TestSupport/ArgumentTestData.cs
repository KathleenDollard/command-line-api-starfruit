using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class ArgumentTestData
    {
        public object Raw { get; set; }
        public IEnumerable<string> Aliases { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHidden { get; set; }
        public int MinArityValues { get; set; }
        public int MaxArityValues { get; set; }
        public bool HasArity { get; set; }
        public HashSet<string> AllowedValues { get; } = new HashSet<string>();
        public Type ArgumentType { get; set; }
        public object DefaultValue { get; set; }
        public bool HasDefault { get; set; }

        //public DefaultValueDescriptor DefaultValue { get; set; }
        public bool Required { get; set; }
    }
}
