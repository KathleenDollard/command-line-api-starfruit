using System;

namespace Model2.Args
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