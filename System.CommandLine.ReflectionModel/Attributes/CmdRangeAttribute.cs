namespace System.CommandLine.ReflectionAppModel
{
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