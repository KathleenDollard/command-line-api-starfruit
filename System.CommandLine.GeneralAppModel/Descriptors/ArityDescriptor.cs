namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArityDescriptor
    {
        public static readonly string MinimumCountName = nameof(MinimumCount);
        public static readonly string MaximumCountName = nameof(MaximumCount);

        public int MinimumCount { get; set; }

        public int MaximumCount { get; set; }
    }
}
