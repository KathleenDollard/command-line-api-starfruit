namespace System.CommandLine.ReflectionAppModel.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class RequiredAttribute : Attribute 
    {
        public RequiredAttribute()
        {
            Value = true;
        }

        public bool Value { get; set; }
    }
}
