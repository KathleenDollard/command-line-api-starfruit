namespace System.CommandLine.GeneralAppModel
{
    public   class DefaultValueAttribute : Attribute
    {
        public DefaultValueAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public object DefaultValue { get;  }
    }
}
