namespace System.CommandLine.ReflectionAppModel.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class DefaultAttribute : Attribute
    {
        public DefaultAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
