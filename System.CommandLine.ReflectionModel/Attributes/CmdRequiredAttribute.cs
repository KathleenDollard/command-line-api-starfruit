namespace System.CommandLine.ReflectionModel
{
    public class CmdRequiredAttribute : Attribute
    {
        public bool Required { get; }

        public CmdRequiredAttribute(bool required = true) 
            => Required = required;
    }
}