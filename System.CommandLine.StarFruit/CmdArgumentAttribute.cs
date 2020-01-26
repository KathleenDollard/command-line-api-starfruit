namespace System.CommandLine.StarFruit
{
    public class CmdArgCountAttribute : Attribute
    {
        public int MinArgCount { get; }
        public int MaxArgCount { get; }

        public CmdArgCountAttribute(int minArgCount, int maxArgCount)
        {
            MinArgCount = minArgCount;
            MaxArgCount = maxArgCount;
        }
    }
}