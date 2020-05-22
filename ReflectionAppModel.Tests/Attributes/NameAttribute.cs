namespace System.CommandLine.ReflectionAppModel.Tests
{
    public   class NameAttribute : Attribute
    {
        public NameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get;  }
    }
}
