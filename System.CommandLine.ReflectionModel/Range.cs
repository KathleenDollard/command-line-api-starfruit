namespace System.CommandLine.ReflectionModel
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

  
}
