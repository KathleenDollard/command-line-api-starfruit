using System;

namespace System.CommandLine.StarFruit
{

    public class CmdDefaultValue : Attribute
    {
        public object DefaultValue { get; set; }

        public CmdDefaultValue(object defaultValue)
            => DefaultValue = defaultValue;
    }

    public class CmdRangeAttribute : Attribute
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public CmdRangeAttribute(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}