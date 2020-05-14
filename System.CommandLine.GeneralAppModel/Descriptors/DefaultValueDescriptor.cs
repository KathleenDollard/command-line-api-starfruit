namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class DefaultValueDescriptor
    {
        private object defaultValue;

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
