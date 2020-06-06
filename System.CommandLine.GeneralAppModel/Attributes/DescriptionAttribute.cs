namespace System.CommandLine.GeneralAppModel
{
    public   class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description { get;  }
    }
}
