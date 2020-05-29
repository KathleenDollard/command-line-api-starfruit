using System.Collections.Generic;

namespace System.CommandLine.ReflectionAppModel.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class OptionAttribute : Attribute
    {
        public OptionAttribute()
        {
        }

        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public string Name { get; set; }
        public string ArgumentName { get; set; }
        public string ArgumentDescription { get; set; }

        // Jon: How should required and option work with option arguments?
        public bool ArgumentRequired { get; set; }
        public bool IsHidden { get; set; }
        public bool OptionRequired { get; set; }
        public bool ArgumentIsHidden { get; set; }
    }
}
