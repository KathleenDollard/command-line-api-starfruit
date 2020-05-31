using System.Collections.Generic;

namespace System.CommandLine.ReflectionAppModel.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Struct)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute()
        {
        }

        public string? Description { get; set; }
        public string[]? Aliases { get; set; }
        public string? Name { get; set; }
        public bool IsHidden { get; set; }
        public bool TreatUnmatchedTokensAsErrors { get; set; } = true;
    }
}
