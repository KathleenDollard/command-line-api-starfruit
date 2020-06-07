using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Rules;

namespace System.CommandLine.GeneralAppModel
{
    public static class FullRules
    {
        public static RuleSetGetCandidatesRule SetFullCandidatesRules( RuleSetGetCandidatesRule rules)
        {
            rules.Rules
                .Add(new DerivedFromRule())
                ;
            return rules;
        }

        public static RuleSetSelectSymbols SetFullSelectSymbolRules( RuleSetSelectSymbols rules)
        {
            rules.Rules
               .Add(new AttributeRule<CommandAttribute>( SymbolType.Command))
               .Add(new NameEndsWithRule("Command", SymbolType.Command))
               .Add(new IsOfTypeRule<Type>(SymbolType.Command))
               .Add(new AttributeRule<ArgumentAttribute>( SymbolType.Argument))
               .Add(new NameEndsWithRule("Argument", SymbolType.Argument))
               .Add(new NameEndsWithRule("Arg", SymbolType.Argument))
               .Add(new RemainingSymbolRule(SymbolType.Option))
            // TODO: .Add(new LabelRule<string>("ComplexUserType", SymbolType.Command))
            ;
            return rules;
        }

        public static RuleSetArgument SetFullArgumentRules( RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new AttributeWithImpliedPropertyRule<DescriptionAttribute, string>())
                .Add(new AttributeWithPropertyValueRule<ArgumentAttribute, string>( "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithImpliedPropertyRule<NameAttribute, string>())
                .Add(new AttributeWithPropertyValueRule<ArgumentAttribute, string>( "Name"))
                .Add(new NameEndsWithRule("Arg"))
                .Add(new NameEndsWithRule("Argument"))
                .Add(new IdentityRule<string>())
                ;

            rules.DefaultValueRules
                .Add(new AttributeWithOptionalValueRule<DefaultValueAttribute, object>( ))
                // DefaultValue on the Argument attribute is not trivial becaues we need to recognize when that is set in a generalized way"
                ;

            rules.RequiredRules
                .Add(new BooleanAttributeRule<RequiredAttribute>())
                .Add(new BooleanAttributeRule<ArgumentAttribute>( "Required"))
            ;

            rules.IsHiddenRules
                .Add(new BooleanAttributeRule<HiddenAttribute>())
                .Add(new BooleanAttributeRule<ArgumentAttribute>( "IsHidden"))
                ;

            rules.AliasRules
                .Add(new AttributeWithImpliedPropertyRule<AliasesAttribute, string[]>())
                .Add(new AttributeWithPropertyValueRule<ArgumentAttribute, string[]>( "Aliases"))
                ;

            rules.AllowedValueRules
                .Add(new AttributeWithImpliedPropertyRule<AllowedValuesAttribute, object[]>())
                ;

            rules.ArityRules
                .Add(new AttributeArityRule<ArityAttribute>( "MinCount", "MaxCount"))
                .Add(new AttributeArityRule<ArityAttribute>( "MinimumCount", "MaximumCount"))
                ;

            return rules;
        }

        public static RuleSetOption SetFullOptionRules( RuleSetOption rules)
        {

            rules.DescriptionRules
                .Add(new AttributeWithImpliedPropertyRule<DescriptionAttribute, string>())
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string>( "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithImpliedPropertyRule<NameAttribute, string>())
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string>( "Name"))
                .Add(new NameEndsWithRule("Option"))
                .Add(new IdentityRule<string>());

            rules.RequiredRules
                .Add(new BooleanAttributeRule<RequiredAttribute>())
                .Add(new BooleanAttributeRule<OptionAttribute>( "OptionRequired"))
             ;

            rules.IsHiddenRules
                .Add(new BooleanAttributeRule<HiddenAttribute>())
                .Add(new BooleanAttributeRule<OptionAttribute>( "IsHidden"))
                ;

            rules.AliasRules
                .Add(new AttributeWithImpliedPropertyRule<AliasesAttribute, string[]>())
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string[]>( "Aliases"))
                ;

            return rules;
        }

        public static RuleSetArgument SetFullOptionArgumentRules( RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string>( "ArgumentDescription", SymbolType.Argument))
                ;

            // The name is used for help
            rules.NameRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, string>( "ArgumentName", SymbolType.Argument))
                ;

            rules.RequiredRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, bool>( "ArgumentRequired", SymbolType.Option))
            ;

            rules.RequiredRules
                .Add(new AttributeWithPropertyValueRule<OptionAttribute, bool>( "ArgumentIsHidden", SymbolType.Option))
            ;

            rules.DefaultValueRules
                .Add(new AttributeWithOptionalValueRule<DefaultValueAttribute, object>( ))
                ;

            // Never hidden, no aliases

            rules.ArityRules
                .Add(new AttributeArityRule<ArityAttribute>( "MinCount", "MaxCount"))
                .Add(new AttributeArityRule<ArityAttribute>( "MinimumCount", "MaximumCount"))
                ;
            return rules;
        }

        public static RuleSetCommand SetFullCommandRules( RuleSetCommand rules)
        {

            rules.DescriptionRules
                .Add(new AttributeWithImpliedPropertyRule<DescriptionAttribute, string>())
                .Add(new AttributeWithPropertyValueRule<CommandAttribute, string>( "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithImpliedPropertyRule<NameAttribute, string>())
                .Add(new AttributeWithPropertyValueRule<CommandAttribute, string>( "Name"))
                .Add(new NameEndsWithRule("Command"))
                .Add(new IdentityRule<string>())
            ;

            rules.TreatUnmatchedTokensAsErrorsRules
                .Add(new BooleanAttributeRule<TreatUnmatchedTokensAsErrorsAttribute>())
                .Add(new AttributeWithPropertyValueRule<CommandAttribute, bool>( "TreatUnmatchedTokensAsErrors"));

            rules.IsHiddenRules
                .Add(new BooleanAttributeRule<HiddenAttribute>())
                .Add(new BooleanAttributeRule<CommandAttribute>( "IsHidden"));

            rules.AliasRules
                .Add(new AttributeWithImpliedPropertyRule<AliasesAttribute, string[]>())
                .Add(new AttributeWithPropertyValueRule<CommandAttribute, string[]>( "Aliases"))
               ;

            return rules;
        }

    }
}
