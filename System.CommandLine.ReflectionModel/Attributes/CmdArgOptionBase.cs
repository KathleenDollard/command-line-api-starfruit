namespace System.CommandLine.ReflectionAppModel
{
    public abstract class CmdArgOptionBaseAttribute : Attribute
    {
        private object defaultValue;

        /// <summary>
        /// Description used for help.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Description used for help.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default value for the argument. If not supplied, the default for the property type is used.
        /// </summary>
        public object DefaultValue { 
            get => defaultValue; 
            set { 
                defaultValue = value;
                HasDefaultValue = true;
            } }

        /// <summary>
        /// Whether there is a default value. This distinguishes between a null default value and no default value
        /// </summary>
        public bool HasDefaultValue { get; private set; }

  
    }
}