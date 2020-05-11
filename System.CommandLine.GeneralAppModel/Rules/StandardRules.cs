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




        public static RuleSet SetStandardArityRules(this RuleSet rules)
           => rules
                .Add(new ComplexAttributeRule<ArityDescriptor>(
                           "Arity", new string[] { "MinArgCount", "MaxArgCount" }));

        public static RuleSet SetStandardDescriptionRules(this RuleSet rules)
           => rules
                .Add(new NamedAttributeRule("Description", "string", "Description"))
                .Add(new NamedAttributeRule("Command", "string", "Description", SymbolType.Command))
                .Add(new NamedAttributeRule("Argument", "string", "Description", SymbolType.Argument))
                .Add(new NamedAttributeRule("Option", "string", "Description", SymbolType.Option))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;

        public static RuleSet SetStandardArgumentRules(this RuleSet rules)
           => rules
                .Add(new NamedAttributeRule("Argument", "string", "Name", SymbolType.Argument))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Arg", SymbolType.Argument))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Argument", SymbolType.Argument));

        public static RuleSet SetStandardSubCommandRules(this RuleSet rules)
           => rules
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Command"))
                .Add(new NamedAttributeRule("Command", "string", "Name"))
            // TODO: .Add(new LabelRule<string>("ComplexUserType"))
            ;
        public static RuleSet SetStandardOptionRules(this RuleSet rules)
            => rules
                 .Add(new RemainingSymbolRule(SymbolType.Option))
             ;

        public static RuleSet SetStandardNameRules(this RuleSet rules)
        {
            rules
                .Add(new NamedAttributeRule("Name", "string", "Name"))
                .Add(new NamedAttributeRule("Option", "string", "Name", SymbolType.Option));
            rules.AddRange(SetStandardArgumentRules, SymbolType.Argument);
            rules.AddRange(SetStandardSubCommandRules, SymbolType.Command);
            rules.Add(new IdentityRule<string>());
            return rules;
        }

        public static RuleSet SetStandardRequiredRules(this RuleSet rules)
           => rules
                .Add(new BoolAttributeRule("Required"))
                .Add(new NamedAttributeRule("Argument", "bool", "Required", SymbolType.Option))
                .Add(new NamedAttributeRule("Option", "bool", "OptionRequired", SymbolType.Option))
                .Add(new NamedAttributeRule("Option", "bool", "ArgumentRequired", SymbolType.Argument));

    }
}
