namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class DefaultValueDescriptor
    {
        private object defaultValue;

        public DefaultValueDescriptor(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public object DefaultValue
        {
            get => defaultValue; 
            set
            {
                defaultValue = value;
                HasDefaultValue = true;
            }
        }

        public bool HasDefaultValue { get; private set; }
    }
}
