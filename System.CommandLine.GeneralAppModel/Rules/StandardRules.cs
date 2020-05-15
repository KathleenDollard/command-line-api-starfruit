using System.CommandLine.GeneralAppModel.Rules;
using System.Threading;

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
               .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Command",SymbolType.Command))
               .Add(new NamedAttributeRule("Command",   SymbolType.Command))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Argument", SymbolType.Argument))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Arg", SymbolType.Argument))
               .Add(new NamedAttributeWithPropertyRule<string>("Argument",  "Name", SymbolType.Argument))
               .Add(new RemainingSymbolRule ( SymbolType.Option))
            // TODO: .Add(new LabelRule<string>("ComplexUserType", SymbolType.Command))
            ;
            return rules;
        }
        public static RuleSetArgument SetStandardArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new NamedAttributeWithPropertyRule<string>("Description",  "Description"))
                .Add(new NamedAttributeWithPropertyRule<string>("Argument",  "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeWithPropertyRule<string>("Name",  "Name"))
                .Add(new NamedAttributeWithPropertyRule<string>("Argument",  "Name", SymbolType.Argument))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Arg", SymbolType.Argument))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Argument", SymbolType.Argument))
                .Add(new IdentityRule<string>())
                ;
            //rules.IsHiddenRule
            //    ;

            rules.RequiredRules
                .Add(new NamedAttributeRule("Required"))
                .Add(new NamedAttributeWithPropertyRule<bool>("Argument",  "Required", SymbolType.Option))
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
                .Add(new NamedAttributeWithPropertyRule<string>("Description",  "Description"))
                .Add(new NamedAttributeWithPropertyRule<string>("Option", "Description", SymbolType.Option))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeWithPropertyRule<string>("Name",  "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Option", SymbolType.Argument))
                .Add(new NamedAttributeWithPropertyRule<string>("Option",  "Name", SymbolType.Option));
            //rules.IsHiddenRule
            //    ;

            rules.RequiredRules
                .Add(new NamedAttributeRule("Required"))
                .Add(new NamedAttributeWithPropertyRule<bool>("Option", "OptionRequired", SymbolType.Option))
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
                .Add(new NamedAttributeWithPropertyRule<string>("Option",  "ArgumentDescription", SymbolType.Option))
                ;
            rules.NameRules
                .Add(new NamedAttributeWithPropertyRule<string>("Option",  "ArgumentName", SymbolType.Argument))
                ;
            //rules.IsHiddenRule
            //    ;

            rules.RequiredRules
                .Add(new NamedAttributeWithPropertyRule<bool>("Option",  "ArgumentRequired", SymbolType.Option))
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
                .Add(new NamedAttributeWithPropertyRule<string>("Description",  "Description"))
                .Add(new NamedAttributeWithPropertyRule<string>("Command",  "Description", SymbolType.Command))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeWithPropertyRule<string>("Name",  "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Command"))
                .Add(new NamedAttributeWithPropertyRule<string>("Command",  "Name"))
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
