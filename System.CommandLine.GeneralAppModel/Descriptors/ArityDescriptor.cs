namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArityDescriptor
    {
        public static readonly string MinimumCountName = nameof(MinimumCount);
        public static readonly string MaximumCountName = nameof(MaximumCount);
        private int minimumNumberOfValues;
        private int maximumNumberOfValues;

        public int MinimumCount
        {
            get => minimumNumberOfValues;
            set
            {
                minimumNumberOfValues = value;
            }
        }

        public int MaximumCount
        {
            get => maximumNumberOfValues;
            set
            {
                maximumNumberOfValues = value;
            }
        }
    }
}
