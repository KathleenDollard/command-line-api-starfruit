using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public static class StandardRules
    {
        public static RuleSet<ArityDescriptor> SetStandardArityRules(RuleSet<ArityDescriptor> rules)
           => rules
                .Add(new ComplexAttributeRule<ArityDescriptor>(
                           "Arity", new string[] { "MinArgCount", "MaxArgCount" }));

        public static RuleSet<string> SetStandardDescriptionRules(RuleSet<string> rules)
           => rules
                .Add(new AttributeRule<string>("Description", "string", "Description"))
                .Add(new AttributeRule<string>("Command", "string", "Description", SymbolType.Command))
                .Add(new AttributeRule<string>("Argument", "string", "Description", SymbolType.Argument))
                .Add(new AttributeRule<string>("Option", "string", "Description", SymbolType.Option))
                .Add(new LabelRule<string>("XmlDocComments"));

        public static RuleSet<string> SetStandardArgumentRules(RuleSet<string> rules)
           => rules
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Arg"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Argument"))
                .Add(new AttributeRule<string>("Argument", "string", "Name"));

        public static RuleSet<string> SetStandardCommandRules(RuleSet<string> rules)
           => rules
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Command"))
                .Add(new AttributeRule<string>("Command", "string", "Name"))
                .Add(new LabelRule<string>("ComplexUserType"));

        public static RuleSet<string> SetStandardNameRules(RuleSet<string> rules)
        {
            rules
                .Add(new AttributeRule<string>("Name", "string", "Name"))
                .Add(new AttributeRule<string>("Option", "string", "Name", SymbolType.Option))
                .Add(new IdentityRule<string>());
            rules.AddRange(SetStandardArgumentRules, SymbolType.Argument);
            rules.AddRange(SetStandardCommandRules, SymbolType.Command);
            return rules;
        }

        public static RuleSet<bool> SetStandardRequiredRules(RuleSet<bool> rules)
           => rules
                .Add(new BoolAttributeRule("Required"))
                .Add(new AttributeRule<bool>("Argument", "bool", "Required", SymbolType.Option))
                .Add(new AttributeRule<bool>("Option", "bool", "OptionRequired", SymbolType.Option))
                .Add(new AttributeRule<bool>("Option", "bool", "ArgumentRequired", SymbolType.Argument));

        public static RuleSet<IEnumerable<CommandDescriptor>> SetStandardSubCommandRules(RuleSet<IEnumerable<CommandDescriptor>> rules)
           => rules
                .Add(new LabelRule<IEnumerable<CommandDescriptor>>("DerivedTypes"));
    }
}
