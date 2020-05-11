using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Rules;

namespace System.CommandLine.GeneralAppModel
{
    public static class StandardRules
    {
        public static Strategy SetStandardArityRules(this Strategy strategy)
        {
            strategy.ArityRules.SetStandardArityRules();
            return strategy;
        }

        public static Strategy SetStandardDescriptionRules(this Strategy strategy)
        {
            strategy.DescriptionRules.SetStandardDescriptionRules();
            return strategy;
        }

        public static Strategy SetStandardArgumentRules(this Strategy strategy)
        {
            strategy.ArgumentRules.SetStandardArgumentRules();
            return strategy;
        }

        public static Strategy SetStandardCommandRules(this Strategy strategy)
        {
            strategy.CommandRules.SetStandardSubCommandRules();
            return strategy;
        }

        public static Strategy SetStandardOptionRules(this Strategy strategy)
        {
            strategy.OptionRules.SetStandardOptionRules();
            return strategy;
        }

        public static Strategy SetStandardNameRules(this Strategy strategy)
        {
            strategy.NameRules.SetStandardNameRules();
            return strategy;
        }

        public static Strategy SetStandardRequiredRules(this Strategy strategy)
        {
            strategy.RequiredRules.SetStandardRequiredRules();
            return strategy;
        }

        public static Strategy SetStandardSubCommandRules(this Strategy strategy)
        {
            strategy.SubCommandRules.SetStandardSubCommandRules();
            return strategy;
        }




        public static RuleSet<ArityDescriptor> SetStandardArityRules(this RuleSet<ArityDescriptor> rules)
           => rules
                .Add(new ComplexAttributeRule<ArityDescriptor>(
                           "Arity", new string[] { "MinArgCount", "MaxArgCount" }));

        public static RuleSet<string> SetStandardDescriptionRules(this RuleSet<string> rules)
           => rules
                .Add(new AttributeRule<string>("Description", "string", "Description"))
                .Add(new AttributeRule<string>("Command", "string", "Description", SymbolType.Command))
                .Add(new AttributeRule<string>("Argument", "string", "Description", SymbolType.Argument))
                .Add(new AttributeRule<string>("Option", "string", "Description", SymbolType.Option))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;

        public static RuleSet<string> SetStandardArgumentRules(this RuleSet<string> rules)
           => rules
                .Add(new AttributeRule<string>("Argument", "string", "Name", SymbolType.Argument))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Arg", SymbolType.Argument))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Argument", SymbolType.Argument));

        public static RuleSet<string> SetStandardSubCommandRules(this RuleSet<string> rules)
           => rules
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Command"))
                .Add(new AttributeRule<string>("Command", "string", "Name"))
            // TODO: .Add(new LabelRule<string>("ComplexUserType"))
            ;
        public static RuleSet<string> SetStandardOptionRules(this RuleSet<string> rules)
            => rules
                 .Add(new RemainingSymbolRule(SymbolType.Option))
             ;

        public static RuleSet<string> SetStandardNameRules(this RuleSet<string> rules)
        {
            rules
                .Add(new AttributeRule<string>("Name", "string", "Name"))
                .Add(new AttributeRule<string>("Option", "string", "Name", SymbolType.Option));
            rules.AddRange(SetStandardArgumentRules, SymbolType.Argument);
            rules.AddRange(SetStandardSubCommandRules, SymbolType.Command);
            rules.Add(new IdentityRule<string>());
            return rules;
        }

        public static RuleSet<bool> SetStandardRequiredRules(this RuleSet<bool> rules)
           => rules
                .Add(new BoolAttributeRule("Required"))
                .Add(new AttributeRule<bool>("Argument", "bool", "Required", SymbolType.Option))
                .Add(new AttributeRule<bool>("Option", "bool", "OptionRequired", SymbolType.Option))
                .Add(new AttributeRule<bool>("Option", "bool", "ArgumentRequired", SymbolType.Argument));

        public static RuleSet<IEnumerable<CommandDescriptor>> SetStandardSubCommandRules(this RuleSet<IEnumerable<CommandDescriptor>> rules)
           => rules
               // TODO: .Add(new LabelRule<IEnumerable<CommandDescriptor>>("DerivedTypes"))
               ;
    }
}
