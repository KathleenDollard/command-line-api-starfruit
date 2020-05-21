namespace System.CommandLine.ReflectionAppModel.Tests
{
    public class DefaultAttribute : Attribute
    {
        public DefaultAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
