namespace System.CommandLine.ReflectionAppModel.Tests
{
    public class ArgumentAttribute : Attribute
    {
        public ArgumentAttribute()
        {
        }

        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsHidden { get; set; }
        public Type ArgumentType { get; set; }
        public bool Required { get; set; }

        //public ArityDescriptor Arity { get; set; }
        //public HashSet<string> AllowedValues { get; } = new HashSet<string>();
        //// TODO: Consider how ArgumentType works when coming from JSON. 
        //public DefaultValueDescriptor DefaultValue { get; set; }
    }
}
