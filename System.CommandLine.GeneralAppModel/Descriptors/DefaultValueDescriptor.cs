namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class DefaultValueDescriptor
    {

        public DefaultValueDescriptor(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public object DefaultValue { get; set; }
    }
}
