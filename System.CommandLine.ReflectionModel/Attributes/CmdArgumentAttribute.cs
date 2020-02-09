﻿namespace System.CommandLine.ReflectionAppModel
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class CmdArgumentAttribute : CmdArgOptionBaseAttribute
    {

        /// <summary>
        /// True if the argument is required.
        /// </summary>
        public bool Required { get; set; }
    }
}