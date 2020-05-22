using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Rules;
using System.Data;
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
               .Add(new NamedAttributeRule("Command", SymbolType.Command))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Command", SymbolType.Command))
               .Add(new IsOfTypeRule<Type>(SymbolType.Command))
               .Add(new NamedAttributeRule("Argument", SymbolType.Argument))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Argument", SymbolType.Argument))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Arg", SymbolType.Argument))
               .Add(new RemainingSymbolRule(SymbolType.Option))
            // TODO: .Add(new LabelRule<string>("ComplexUserType", SymbolType.Command))
            ;
            return rules;
        }
 
        public static RuleSetArgument SetStandardArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new NamedAttributeWithPropertyRule<string>("Description", "Description"))
                .Add(new NamedAttributeWithPropertyRule<string>("Argument", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeWithPropertyRule<string>("Name", "Name"))
                .Add(new NamedAttributeWithPropertyRule<string>("Argument", "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Arg"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Argument"))
                .Add(new IdentityRule<string>())
                ;
            rules.DefaultRules
                .Add(new OptionalValueAttributeRule<object>("Default", "Value"))
                // Default on the Argument attribute is not trivial becaues we need to recognize when that is set in a generalized way"
                ;

            rules.RequiredRules
                .Add(new NamedAttributeWithPropertyRule<bool>("Required", "Value"))
                .Add(new NamedAttributeWithPropertyRule<bool>("Argument", "Required"))
            ;

            rules.IsHiddenRules
                .Add(new NamedAttributeWithPropertyRule<bool>("Hidden", "Value"))
                .Add(new NamedAttributeWithPropertyRule<bool>("Argument", "IsHidden"))
                ;

            rules.AliasRules
                .Add(new NamedAttributeWithPropertyRule<string>("Aliases", "Aliases"))
                .Add(new NamedAttributeWithPropertyRule<string>("Argument", "Aliases"))
                ;

            rules.ArityRules
                .Add(new ComplexAttributeRule("Arity")
                {
                    PropertyNamesAndTypes = new List<ComplexAttributeRule.NameAndType>()
                    { 
                        new ComplexAttributeRule.NameAndType(ArityDescriptor.MinimumCountName,propertyName: "MinCount", propertyType: typeof(int)),
                        new ComplexAttributeRule.NameAndType(ArityDescriptor.MaximumCountName,propertyName: "MaxCount", propertyType: typeof(int))
                    }
                })
                .Add(new ComplexAttributeRule("Arity")
                {
                    PropertyNamesAndTypes = new List<ComplexAttributeRule.NameAndType>()
                    {
                        new ComplexAttributeRule.NameAndType(ArityDescriptor.MinimumCountName, propertyName: "MinimumCount", propertyType: typeof(int)),
                        new ComplexAttributeRule.NameAndType(ArityDescriptor.MaximumCountName, propertyName: "MaximumCount", propertyType: typeof(int))
                    }
                 });

            return rules;
        }

        public static RuleSetOption SetStandardOptionRules(this RuleSetOption rules)
        {

            rules.DescriptionRules
                .Add(new NamedAttributeWithPropertyRule<string>("Description", "Description"))
                .Add(new NamedAttributeWithPropertyRule<string>("Option", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeWithPropertyRule<string>("Name", "Name"))
                .Add(new NamedAttributeWithPropertyRule<string>("Option", "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Option"))
                .Add(new IdentityRule<string>());

            rules.RequiredRules
                .Add(new NamedAttributeWithPropertyRule<bool>("Required", "Value"))
                .Add(new NamedAttributeWithPropertyRule<bool>("Option", "OptionRequired"))
             ;

            rules.IsHiddenRules
                .Add(new NamedAttributeWithPropertyRule<bool>("Hidden", "Value"))
                .Add(new NamedAttributeWithPropertyRule<bool>("Option", "IsHidden"))
                ;

            rules.AliasRules
                .Add(new NamedAttributeWithPropertyRule<string>("Aliases", "Aliases"))
                .Add(new NamedAttributeWithPropertyRule<string>("Option", "Aliases"))
                ;

            return rules;
        }

        public static RuleSetArgument SetStandardOptionArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new NamedAttributeWithPropertyRule<string>("Option", "ArgumentDescription", SymbolType.Argument))
                ;
            rules.NameRules
                .Add(new NamedAttributeWithPropertyRule<string>("Option", "ArgumentName", SymbolType.Argument))
                ;

            rules.RequiredRules
                .Add(new NamedAttributeWithPropertyRule<bool>("Option", "ArgumentRequired", SymbolType.Option))
            ;

            //rules.ArityRule
            //    .Add(new ComplexAttributeRule<ArityDescriptor>(
            //               "Arity", new string[] { "MinArgCount", "MaxArgCount" }));
            // ;

            return rules;
        }

        public static RuleSetCommand SetStandardCommandRules(this RuleSetCommand rules)
        {

            rules.DescriptionRules
                .Add(new NamedAttributeWithPropertyRule<string>("Description", "Description"))
                .Add(new NamedAttributeWithPropertyRule<string>("Command", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeWithPropertyRule<string>("Name", "Name"))
                .Add(new NamedAttributeWithPropertyRule<string>("Command", "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Command"))
                .Add(new IdentityRule<string>())
            ;

            rules.TreatUnmatchedTokensAsErrorsRules
                .Add(new NamedAttributeWithPropertyRule<bool>("TreatUnmatchedTokensAsErrors", "Value"))
                .Add(new NamedAttributeWithPropertyRule<bool>("Command", "TreatUnmatchedTokensAsErrors"));

            rules.IsHiddenRules
                .Add(new NamedAttributeWithPropertyRule<bool>("Hidden", "Value"))
                .Add(new NamedAttributeWithPropertyRule<bool>("Command", "IsHidden"));

            rules.AliasRules
                .Add(new NamedAttributeWithPropertyRule<string>("Aliases", "Aliases"))
                 .Add(new NamedAttributeWithPropertyRule<string>("Command", "Aliases"))
               ;

            return rules;
        }

    }
}
