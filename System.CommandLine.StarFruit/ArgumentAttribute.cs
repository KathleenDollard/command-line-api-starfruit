namespace System.CommandLine.StarFruit
{

    public class CmdArgumentAttribute: Attribute
    {  }

    public class CmdDefaultValueAttribute : Attribute
    {
        public object DefaultValue { get; }

        public CmdDefaultValueAttribute(object defaultValue)
            => DefaultValue = defaultValue;
    }

    public class CmdRangeAttribute : Attribute
    {
        public int MinValue { get; }
        public int MaxValue { get; }

        public CmdRangeAttribute(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}