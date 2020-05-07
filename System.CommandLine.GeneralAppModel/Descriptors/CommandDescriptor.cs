using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class CommandDescriptor
    {
        public object Raw { get; set; }
        public IEnumerable<string> Aliases { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHidden { get; set; }
        public List<ArgumentDescriptor> Arguments { get; } = new List<ArgumentDescriptor>();
        public List<OptionDescriptor> Options { get; } = new List<OptionDescriptor>();
        public List<CommandDescriptor> SubCommands { get; } = new List<CommandDescriptor>();
        public bool TreatUnmatchedTokensAsErrors { get; set; } = true;

    }
}
