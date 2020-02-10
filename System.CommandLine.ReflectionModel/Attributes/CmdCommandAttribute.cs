namespace System.CommandLine.ReflectionModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter | AttributeTargets.Property )]
    public class CmdCommandAttribute : Attribute
    {
        /// <summary>
        /// Name for the argument. Defaults to the property name. It will be changed to lower kebab case and prefixed with --.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description used for help.
        /// </summary>
        public string Description { get; set; }
    }
}
