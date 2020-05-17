namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArityDescriptor
    {
        public static readonly string MinimumCountName = nameof(MinimumNumberOfValues);
        public static readonly string MaximumCountName = nameof(MaximumNumberOfValues);
        private int minimumNumberOfValues;
        private int maximumNumberOfValues;

        public bool IsSet { get; private set; }
        public int MinimumNumberOfValues
        {
            get => minimumNumberOfValues;
            set
            {
                minimumNumberOfValues = value;
                IsSet = true;
            }
        }

        public int MaximumNumberOfValues
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
