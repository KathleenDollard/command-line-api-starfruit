namespace System.CommandLine.GeneralAppModel
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ArityAttribute : Attribute
    {

        public ArityAttribute(int minimumCount , int maximumCount = int.MaxValue )
        {
            MinimumCount = minimumCount;
            MaximumCount = maximumCount;
        }

        public int MinimumCount { get;  }
        public int MaximumCount { get;  }
    }
}
