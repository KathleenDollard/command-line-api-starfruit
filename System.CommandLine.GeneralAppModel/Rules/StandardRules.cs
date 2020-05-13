using System.CommandLine.GeneralAppModel.Rules;

namespace System.CommandLine.GeneralAppModel
{
    public static class StandardRules
    {

        public static Strategy SetStandardSelectSymbolRules(this Strategy strategy)
        {
            strategy.SelectSymbolRules.SetStandardSelectSymbolRules();
            return strategy;
        }

        public static Strategy SetStandardArgumentRules(this Strategy strategy)
        {
            strategy.ArgumentRules.SetStandardArgumentRules();
            return strategy;
        }

        public static Strategy SetStandardCommandRules(this Strategy strategy)
        {
            strategy.CommandRules.SetStandardCommandRules();
            return strategy;
        }

        public static Strategy SetStandardOptionRules(this Strategy strategy)
        {
            strategy.OptionRules.SetStandardOptionRules();
            return strategy;
        }

        public static RuleSetSelectSymbols SetStandardSelectSymbolRules(this RuleSetSelectSymbols rules)
        {
            rules.Rules
               .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Command",SymbolType.Command))
               .Add(new NamedAttributeRule("Command", "string", "Name", SymbolType.Command))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Argument", SymbolType.Argument))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Arg", SymbolType.Argument))
               .Add(new NamedAttributeRule("Argument", "string", "Name", SymbolType.Option))
               .Add(new RemainingSymbolRule ( SymbolType.Option))
            // TODO: .Add(new LabelRule<string>("ComplexUserType", SymbolType.Command))
            ;
            return rules;
        }
        public static RuleSetArgument SetStandardArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new NamedAttributeRule("Description", "string", "Description"))
                .Add(new NamedAttributeRule("Argument", "string", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeRule("Name", "string", "Name"))
                .Add(new NamedAttributeRule("Argument", "string", "Name", SymbolType.Argument))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Arg", SymbolType.Argument))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Argument", SymbolType.Argument))
                .Add(new IdentityRule<string>())
                ;
            //rules.IsHiddenRule
            //    ;

            rules.RequiredRules
                .Add(new BoolAttributeRule("Required"))
                .Add(new NamedAttributeRule("Argument", "bool", "Required", SymbolType.Option))
            ;

            //rules.AliasesRules
            //    ;

            //rules.ArityRule
            //    .Add(new ComplexAttributeRule<ArityDescriptor>(
            //               "Arity", new string[] { "MinArgCount", "MaxArgCount" }));
            // ;

            return rules;
        }

        public static RuleSetOption SetStandardOptionRules(this RuleSetOption rules)
        {

            rules.DescriptionRules
                .Add(new NamedAttributeRule("Description", "string", "Description"))
                .Add(new NamedAttributeRule("Option", "string", "Description", SymbolType.Option))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeRule("Name", "string", "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Option", SymbolType.Argument))
                .Add(new NamedAttributeRule("Option", "string", "Name", SymbolType.Option));
            //rules.IsHiddenRule
            //    ;

            rules.RequiredRules
                .Add(new BoolAttributeRule("Required"))
                .Add(new NamedAttributeRule("Option", "bool", "OptionRequired", SymbolType.Option))
             ;

            //rules.AliasesRules
            //    ;

            //rules.IsHiddenRules
            //    ;


            return rules;
        }

        public static RuleSetArgument SetStandardOptionArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new NamedAttributeRule ("Option", "string", "ArgumentDescription", SymbolType.Option))
                ;
            rules.NameRules
                .Add(new NamedAttributeRule("Option", "string", "ArgumentName", SymbolType.Argument))
                ;
            //rules.IsHiddenRule
            //    ;

            rules.RequiredRules
                .Add(new NamedAttributeRule("Option", "bool", "ArgumentRequired", SymbolType.Option))
            ;

            //rules.AliasesRules
            //    ;

            //rules.ArityRule
            //    .Add(new ComplexAttributeRule<ArityDescriptor>(
            //               "Arity", new string[] { "MinArgCount", "MaxArgCount" }));
            // ;

            return rules;
        }

        public static RuleSetCommand SetStandardCommandRules(this RuleSetCommand rules)
        {

            rules.DescriptionRules
                .Add(new NamedAttributeRule("Description", "string", "Description"))
                .Add(new NamedAttributeRule("Command", "string", "Description", SymbolType.Command))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeRule("Name", "string", "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.Suffix, "Command"))
                .Add(new NamedAttributeRule("Command", "string", "Name"))
                .Add(new IdentityRule<string>())
            ;
            //rules.IsHiddenRules
            //    ;

            //rules.AliasesRules
            //    ;

            //rules.IsHiddenRules
            //    ;


            return rules;
        }

    }
}
