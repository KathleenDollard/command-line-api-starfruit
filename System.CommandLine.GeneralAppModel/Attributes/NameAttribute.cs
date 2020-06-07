namespace System.CommandLine.GeneralAppModel
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
