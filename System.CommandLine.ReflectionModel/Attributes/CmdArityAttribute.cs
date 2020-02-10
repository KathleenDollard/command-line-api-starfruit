namespace System.CommandLine.ReflectionModel
{
    /// <summary>
    /// When the argument type is a collection, this specifies the number of arguments allowed. This is not a common case.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class CmdArityAttribute : Attribute
    {
        /// <summary>
        /// When the option type is a collection, you can specify the minimum number of entries. Defaults to zero?
        /// </summary>
        public int MinArgCount { get; set; }

        /// <summary>
        /// When the option type is a collection, you can specify the maximum number of entries. Defaults is no maximum.
        /// </summary>
        public int MaxArgCount { get; set; }
    }
}
