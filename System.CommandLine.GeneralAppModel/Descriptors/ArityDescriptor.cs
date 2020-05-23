namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArityDescriptor
    {
        public static readonly string MinimumCountName = nameof(MinimumCount);
        public static readonly string MaximumCountName = nameof(MaximumCount);
        private int minimumNumberOfValues;
        private int maximumNumberOfValues;

        public bool IsSet { get; private set; }
        public int MinimumCount
        {
            get => minimumNumberOfValues;
            set
            {
                minimumNumberOfValues = value;
                IsSet = true;
            }
        }

        public int MaximumCount
        {
            get => maximumNumberOfValues;
            set
            {
                maximumNumberOfValues = value;
                IsSet = true;
            }
        }
    }
}
