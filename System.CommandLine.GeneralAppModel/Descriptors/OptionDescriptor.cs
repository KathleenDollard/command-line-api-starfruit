using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class OptionDescriptor
    {
        public object Raw { get; set; }
        public IEnumerable<string> Aliases { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHidden { get; set; }
        public IEnumerable< ArgumentDescriptor> Arguments { get; set; }
        public bool Required { get; set; }
    }
}
