namespace System.CommandLine.ReflectionAppModel.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ArityAttribute : Attribute
    {
        private int minimumCount = 0;
        private int maximumCount = int.MaxValue ;
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
