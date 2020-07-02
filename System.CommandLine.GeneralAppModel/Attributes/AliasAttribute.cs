using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This is an alternate way of defining aliases on the command. 
    /// </summary>
    /// <remarks>
    /// If we keep this attribute approach, the AliasesAttribute should probably be removed in favor
    /// of aliases in Symbol attributes. 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AliasAttribute : Attribute
    {
        public AliasAttribute(string alias, string optionName)
        {
            Alias = alias;
            OptionName = optionName;
        }
        public string OptionName { get; }
        public string Alias { get; }
    }


}
