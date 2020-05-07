using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public class Strategy
    {

        public RuleSet<ArityDescriptor> ArityRules { get; }
        public RuleSet<string> DescriptionRules { get; }
        public RuleSet<string> ArgumentRules { get; }
        public RuleSet<string> CommandRules { get; }
        public RuleSet<string> AliasRules { get; }
        public RuleSet<string> NameRules { get; }
        public RuleSet<bool> RequiredRules { get; }
        public RuleSet<bool> HiddenRules { get; }
        public RuleSet<DefaultValueDescriptor> DefaultRules { get; }
        public RuleSet<IEnumerable<CommandDescriptor>> SubCommandRules { get; }

        public void SetAllStandardRules()
        {
            StandardRules.SetStandardArityRules(ArityRules);
            StandardRules.SetStandardDescriptionRules(DescriptionRules);
            StandardRules.SetStandardArgumentRules(ArgumentRules);
            StandardRules.SetStandardCommandRules(CommandRules);
            StandardRules.SetStandardNameRules(NameRules);
            StandardRules.SetStandardRequiredRules(RequiredRules);
            StandardRules.SetStandardSubCommandRules(SubCommandRules);
        }
    }
}
