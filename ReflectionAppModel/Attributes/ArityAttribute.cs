namespace System.CommandLine.ReflectionAppModel.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ArityAttribute : Attribute
    {
        private int minimumCount;
        private int maximumCount;
        private bool isSet;

        public ArityAttribute()
        {
        }

        public int MinimumCount
        {
            get => minimumCount;
            set
            {
                isSet = true;
                minimumCount = value;
            }
        }
        public int MaximumCount
        {
            get => maximumCount;
            set
            {
                isSet = true;
                maximumCount = value;
            }
        }
    }
}
