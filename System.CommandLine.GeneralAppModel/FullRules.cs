using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public static class FullRules
    {

        public static Strategy SetFullSelectSymbolRules(this Strategy strategy)
        {
            strategy.SelectSymbolRules.SetFullSelectSymbolRules();
            return strategy;
        }

        public static Strategy SetFullArgumentRules(this Strategy strategy)
        {
            strategy.ArgumentRules.SetFullArgumentRules();
            return strategy;
        }

        public static Strategy SetFullCommandRules(this Strategy strategy)
        {
            strategy.CommandRules.SetFullCommandRules();
            return strategy;
        }

        public static Strategy SetFullOptionRules(this Strategy strategy)
        {
            strategy.OptionRules.SetFullOptionRules();
            return strategy;
        }

        public static RuleSetSelectSymbols SetFullSelectSymbolRules(this RuleSetSelectSymbols rules)
        {
            rules.Rules
               .Add(new AttributeRule("Command", SymbolType.Command))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Command", SymbolType.Command))
               .Add(new IsOfTypeRule<Type>(SymbolType.Command))
               .Add(new AttributeRule("Argument", SymbolType.Argument))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Argument", SymbolType.Argument))
               .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Arg", SymbolType.Argument))
               .Add(new RemainingSymbolRule(SymbolType.Option))
            // TODO: .Add(new LabelRule<string>("ComplexUserType", SymbolType.Command))
            ;
            return rules;
        }
 
        public static RuleSetArgument SetFullArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new AttributeWithPropertyRule<string>("Description", "Description"))
                .Add(new AttributeWithPropertyRule<string>("Argument", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithPropertyRule<string>("Name", "Name"))
                .Add(new AttributeWithPropertyRule<string>("Argument", "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Arg"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Argument"))
                .Add(new IdentityRule<string>())
                ;
            rules.DefaultRules
                .Add(new OptionalValueAttributeRule<object>("Default", "Value"))
                // Default on the Argument attribute is not trivial becaues we need to recognize when that is set in a generalized way"
                ;

            rules.RequiredRules
                .Add(new AttributeWithPropertyRule<bool>("Required"))
                .Add(new AttributeWithPropertyRule<bool>("Argument", "Required"))
            ;

            rules.IsHiddenRules
                .Add(new AttributeWithPropertyRule<bool>("Hidden"))
                .Add(new AttributeWithPropertyRule<bool>("Argument", "IsHidden"))
                ;

            rules.AliasRules
                .Add(new AttributeWithPropertyRule<string>("Aliases", "Aliases"))
                .Add(new AttributeWithPropertyRule<string>("Argument", "Aliases"))
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

        public static RuleSetOption SetFullOptionRules(this RuleSetOption rules)
        {

            rules.DescriptionRules
                .Add(new AttributeWithPropertyRule<string>("Description", "Description"))
                .Add(new AttributeWithPropertyRule<string>("Option", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithPropertyRule<string>("Name", "Name"))
                .Add(new AttributeWithPropertyRule<string>("Option", "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Option"))
                .Add(new IdentityRule<string>());

            rules.RequiredRules
                .Add(new AttributeWithPropertyRule<bool>("Required"))
                .Add(new AttributeWithPropertyRule<bool>("Option", "OptionRequired"))
             ;

            rules.IsHiddenRules
                .Add(new AttributeWithPropertyRule<bool>("Hidden"))
                .Add(new AttributeWithPropertyRule<bool>("Option", "IsHidden"))
                ;

            rules.AliasRules
                .Add(new AttributeWithPropertyRule<string>("Aliases", "Aliases"))
                .Add(new AttributeWithPropertyRule<string>("Option", "Aliases"))
                ;

            return rules;
        }

        public static RuleSetArgument SetStandardOptionArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new AttributeWithPropertyRule<string>("Option", "ArgumentDescription", SymbolType.Argument))
                ;

            // The name is used for help
            rules.NameRules
                .Add(new AttributeWithPropertyRule<string>("Option", "ArgumentName", SymbolType.Argument))
                ;

            rules.RequiredRules
                .Add(new AttributeWithPropertyRule<bool>("Option", "ArgumentRequired", SymbolType.Option))
            ;

            rules.RequiredRules
                .Add(new AttributeWithPropertyRule<bool>("Option", "ArgumentIsHidden", SymbolType.Option))
            ;

            rules.DefaultRules
                .Add(new OptionalValueAttributeRule<object>("Default", "Value"))
                ;
 
            // Never hidden, no aliases

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

        public static RuleSetCommand SetFullCommandRules(this RuleSetCommand rules)
        {

            rules.DescriptionRules
                .Add(new AttributeWithPropertyRule<string>("Description", "Description"))
                .Add(new AttributeWithPropertyRule<string>("Command", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithPropertyRule<string>("Name", "Name"))
                .Add(new AttributeWithPropertyRule<string>("Command", "Name"))
                .Add(new NamePatternRule(StringContentsRule.StringPosition.EndsWith, "Command"))
                .Add(new IdentityRule<string>())
            ;

            rules.TreatUnmatchedTokensAsErrorsRules
                .Add(new AttributeWithPropertyRule<bool>("TreatUnmatchedTokensAsErrors", "Value"))
                .Add(new AttributeWithPropertyRule<bool>("Command", "TreatUnmatchedTokensAsErrors"));

            rules.IsHiddenRules
                .Add(new AttributeWithPropertyRule<bool>("Hidden"))
                .Add(new AttributeWithPropertyRule<bool>("Command", "IsHidden"));

            rules.AliasRules
                .Add(new AttributeWithPropertyRule<string>("Aliases", "Aliases"))
                 .Add(new AttributeWithPropertyRule<string>("Command", "Aliases"))
               ;

            return rules;
        }

    }
}
