using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public class Strategy
    {

        public RuleSet<ArityDescriptor> ArityRules { get; } = new RuleSet<ArityDescriptor>();
        public RuleSet<string> DescriptionRules { get; } = new RuleSet<string>();
        public RuleSet<string> ArgumentRules { get; } = new RuleSet<string>();
        public RuleSet<string> CommandRules { get; } = new RuleSet<string>();
        public RuleSet<string> AliasRules { get; } = new RuleSet<string>();
        public RuleSet<string> NameRules { get; } = new RuleSet<string>();
        public RuleSet<bool> RequiredRules { get; } = new RuleSet<bool>();
        public RuleSet<bool> HiddenRules { get; } = new RuleSet<bool>();
        public RuleSet<DefaultValueDescriptor> DefaultRules { get; } = new RuleSet<DefaultValueDescriptor>();
        public RuleSet<IEnumerable<CommandDescriptor>> SubCommandRules { get; } = new RuleSet<IEnumerable<CommandDescriptor>>();

  
    }
}
