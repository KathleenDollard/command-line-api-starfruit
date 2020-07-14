using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Rules;

namespace System.CommandLine.GeneralAppModel
{
    public static class StandardRules
    {
        public static RuleSetDescriptorContext SetDescriptorContextRules(RuleSetDescriptorContext rules)
        {
            rules.DescriptionSourceRules
                .Add(new AttributeWithImpliedPropertyRule<DescriptionSourceAttribute, Type>())
                ;
            return rules;
        }

        public static RuleSetGetCandidatesRule SetCandidatesRules(RuleSetGetCandidatesRule rules)
        {
            rules.Rules
                .Add(new DerivedFromRule())
                ;
            return rules;
        }

        public static RuleSetSelectSymbols SetSelectSymbolRules(RuleSetSelectSymbols rules)
        {
            rules.Rules
               .Add(new AttributeRule<CommandAttribute>(SymbolType.Command))
               .Add(new NameEndsWithRule("Command", SymbolType.Command))
               .Add(new IsOfTypeRule<Type>(SymbolType.Command))
               .Add(new AttributeRule<ArgumentAttribute>(SymbolType.Argument))
               .Add(new NameEndsWithRule("Arg", SymbolType.Argument))
               .Add(new RemainingSymbolRule(SymbolType.Option))
            // TODO: .Add(new LabelRule<string>("ComplexUserType", SymbolType.Command))
            ;
            return rules;
        }

        public static RuleSetArgument SetArgumentRules(RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new AttributeWithPropertyValueRule<ArgumentAttribute, string>("Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithPropertyValueRule<ArgumentAttribute, string>("Name"))
                .Add(new NameEndsWithRule("Arg"))
                .Add(new IdentityRule<string>())
                ;

            //rules.CommandLineNameRules  // None for now
            //;

            rules.RequiredRules
                .Add(new BooleanAttributeRule<ArgumentAttribute>("Required"))
            ;

            rules.IsHiddenRules
                .Add(new BooleanAttributeRule<ArgumentAttribute>("IsHidden"))
                ;

            rules.AliasRules
                .Add(new AttributeWithPropertyValueRule<ArgumentAttribute, string[]>("Aliases"))
                .Add(new AttributeWithImpliedPropertyRule<AliasesAttribute, string[]>())
                ;

            rules.DefaultValueRules
                .Add(new AttributeWithOptionalValueRule<DefaultValueAttribute, object>())
                // DefaultValue on the Argument attribute is not trivial becaues we need to recognize when that is set in a generalized way"
                ;

            rules.AllowedValuesRules
                .Add(new AttributeWithImpliedPropertyRule<AllowedValuesAttribute, object[]>())
                ;

            rules.ArityRules
                .Add(new AttributeArityRule<ArityAttribute>("MinimumCount", "MaximumCount"))
                ;

            return rules;
        }

        public static RuleSetOption SetOptionRules(RuleSetOption rules)
        {

            rules.DescriptionRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string>("Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string>("Name"))
                .Add(new NameEndsWithRule("Option"))
                .Add(new IdentityRule<string>());

            rules.CommandLineNameRules
                .Add(new MorphNameCasingRule(MorphNameCasingRule.StringCasing.LowerKebab))
                .Add(new MorphNamePrefixRule("--")) 
            ;

            rules.RequiredRules
                .Add(new BooleanAttributeRule<OptionAttribute>("OptionRequired"))
             ;

            rules.IsHiddenRules
                .Add(new BooleanAttributeRule<OptionAttribute>("IsHidden"))
                ;

            rules.AliasRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string[]>("Aliases"))
                .Add(new AttributeWithImpliedPropertyRule<AliasesAttribute, string[]>())
                ;

            return rules;
        }

        public static RuleSetArgument SetOptionArgumentRules(RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string>("ArgumentDescription", SymbolType.Argument))
                ;

            // The name is used for help
            rules.NameRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string>("ArgumentName", SymbolType.Argument))
                ;

            rules.RequiredRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, bool>("ArgumentRequired", SymbolType.Option))
            ;

            rules.IsHiddenRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, bool>("ArgumentIsHidden", SymbolType.Option))
            ;

            rules.DefaultValueRules
                .Add(new AttributeWithOptionalValueRule<DefaultValueAttribute, object>())
                ;

            // Never hidden, no aliases

            rules.ArityRules
                .Add(new AttributeArityRule<ArityAttribute>("MinimumCount", "MaximumCount"))
                ;
            return rules;
        }

        public static RuleSetCommand SetCommandRules(RuleSetCommand rules)
        {

            rules.DescriptionRules
                .Add(new AttributeWithPropertyValueRule<CommandAttribute, string>("Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithPropertyValueRule<CommandAttribute, string>("Name"))
                .Add(new NameEndsWithRule("Command"))
                .Add(new IdentityRule<string>())
            ;

            rules.CommandLineNameRules
                .Add(new MorphNameCasingRule(MorphNameCasingRule.StringCasing.LowerKebab))
            ;

            rules.TreatUnmatchedTokensAsErrorsRules
                .Add(new AttributeWithPropertyValueRule<CommandAttribute, bool>("TreatUnmatchedTokensAsErrors"));

            rules.IsHiddenRules
                .Add(new BooleanAttributeRule<CommandAttribute>("IsHidden"));

            rules.AliasRules
                .Add(new AttributeWithPropertyValueRule<CommandAttribute, string[]>("Aliases"))
               ;

            rules.InvokeMethodRules
               .Add(new NamedInvokeMethodRule("Invoke"))
               ;
            
            return rules;
        }
    }
}
