namespace System.CommandLine.ReflectionAppModel
{
    public class CmdNameAttribute : Attribute
    {
        public string Name { get;  }
        public CmdNameAttribute(string name) 
            => Name = name;
    }
}