namespace System.CommandLine.StarFruit
{
    public class CmdDescriptionAttribute : Attribute
    {
        public string Description { get; }

        public CmdDescriptionAttribute(string description) 
            => Description = description;
    }
}