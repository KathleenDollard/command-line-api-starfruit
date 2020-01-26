namespace System.CommandLine.StarFruit
{
    public class Range
    {
        public static Range<T> Create<T>(T min, T max)
            where T : IComparable<T>
            => new Range<T>()
            {
                Min = min,
                Max = max
            };
    }

    public class Range<T> : Range
        where T : IComparable<T>
    {

        public T Min { get; internal set; }
        public T Max { get; internal set; }

    }

    public class Default
    {
        public static Default<T> Create<T>(T defaultValue)
            => new Default<T>()
            {
                Value = defaultValue
            };
    }

    public class Default<T> : Default
    {
        public T Value { get; internal set; }
    }

}
