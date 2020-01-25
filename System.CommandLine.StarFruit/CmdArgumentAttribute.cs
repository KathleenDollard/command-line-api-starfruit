namespace System.CommandLine.StarFruit
{
    public class CmdArgumentAttribute : Attribute
    {
        public string Description { get; }
        public int MinArgCount { get; set; }
        public int MaxArgCount { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public object DefaultValue { get; set; }

        public CmdArgumentAttribute()
        { }
    }
}