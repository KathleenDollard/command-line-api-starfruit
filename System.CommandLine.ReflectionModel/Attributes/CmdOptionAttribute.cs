namespace System.CommandLine.ReflectionAppModel
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class CmdOptionAttribute : CmdArgOptionBaseAttribute
    {

        /// <summary>
        /// Description used for help.
        /// </summary>
        public string ArgumentDescription { get; set; }

        /// <summary>
        /// If true, the argument for the option is required
        /// </summary>
        public bool ArgumentRequired { get; set; }


        /// <summary>
        /// If true, the option is required
        /// </summary>
        public bool OptionRequired { get; set; }
    }
}
