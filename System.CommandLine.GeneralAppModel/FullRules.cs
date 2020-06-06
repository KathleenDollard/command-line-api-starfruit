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
               .Add(new StronglyTypedAttributeRule<CommandAttribute>( SymbolType.Command))
               .Add(new NameEndsWithRule("Command", SymbolType.Command))
               .Add(new IsOfTypeRule<Type>(SymbolType.Command))
               .Add(new StronglyTypedAttributeRule<ArgumentAttribute>( SymbolType.Argument))
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
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<DescriptionAttribute, string>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<ArgumentAttribute, string>( "Description"))
                //.Add(new StronglyTypedAttributeWithImpliedPropertyRule<DescriptionAttribute, string>())
                //.Add(new StronglyTypedAttributeWithPropertyValueRule<ArgumentAttribute, string>("Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<NameAttribute, string>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<ArgumentAttribute, string>( "Name"))
                .Add(new NameEndsWithRule("Arg"))
                .Add(new NameEndsWithRule("Argument"))
                .Add(new IdentityRule<string>())
                ;

            rules.DefaultValueRules
                .Add(new StronglyTypedAttributeWithOptionalValueRule<DefaultValueAttribute, object>( ))
                // DefaultValue on the Argument attribute is not trivial becaues we need to recognize when that is set in a generalized way"
                ;

            rules.RequiredRules
                .Add(new StronglyTypedBooleanAttributeRule<RequiredAttribute>())
                .Add(new StronglyTypedBooleanAttributeRule<ArgumentAttribute>( "Required"))
            ;

            rules.IsHiddenRules
                .Add(new StronglyTypedBooleanAttributeRule<HiddenAttribute>())
                .Add(new StronglyTypedBooleanAttributeRule<ArgumentAttribute>( "IsHidden"))
                ;

            rules.AliasRules
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<AliasesAttribute, string[]>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<ArgumentAttribute, string[]>( "Aliases"))
                ;

            rules.AllowedValueRules
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<AllowedValuesAttribute, object[]>())
                ;

            rules.ArityRules
                .Add(new StronglyTypedAttributeArityRule<ArityAttribute>( "MinCount", "MaxCount"))
                .Add(new StronglyTypedAttributeArityRule<ArityAttribute>( "MinimumCount", "MaximumCount"))
                ;

            return rules;
        }

        public static RuleSetOption SetFullOptionRules( RuleSetOption rules)
        {

            rules.DescriptionRules
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<DescriptionAttribute, string>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<OptionAttribute, string>( "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<NameAttribute, string>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<OptionAttribute, string>( "Name"))
                .Add(new NameEndsWithRule("Option"))
                .Add(new IdentityRule<string>());

            rules.RequiredRules
                .Add(new StronglyTypedBooleanAttributeRule<RequiredAttribute>())
                .Add(new StronglyTypedBooleanAttributeRule<OptionAttribute>( "OptionRequired"))
             ;

            rules.IsHiddenRules
                .Add(new StronglyTypedBooleanAttributeRule<HiddenAttribute>())
                .Add(new StronglyTypedBooleanAttributeRule<OptionAttribute>( "IsHidden"))
                ;

            rules.AliasRules
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<AliasesAttribute, string[]>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<OptionAttribute, string[]>( "Aliases"))
                ;

            return rules;
        }

        public static RuleSetArgument SetFullOptionArgumentRules( RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new StronglyTypedAttributeWithPropertyValueRule<OptionAttribute, string>( "ArgumentDescription", SymbolType.Argument))
                ;

            // The name is used for help
            rules.NameRules
                .Add(new StronglyTypedAttributeWithPropertyValueRule<OptionAttribute, string>( "ArgumentName", SymbolType.Argument))
                ;

            rules.RequiredRules
                .Add(new StronglyTypedAttributeWithPropertyValueRule<OptionAttribute, bool>( "ArgumentRequired", SymbolType.Option))
            ;

            rules.RequiredRules
                .Add(new StronglyTypedAttributeWithPropertyValueRule<OptionAttribute, bool>( "ArgumentIsHidden", SymbolType.Option))
            ;

            rules.DefaultValueRules
                .Add(new StronglyTypedAttributeWithOptionalValueRule<DefaultValueAttribute, object>( ))
                ;

            // Never hidden, no aliases

            rules.ArityRules
                .Add(new StronglyTypedAttributeArityRule<ArityAttribute>( "MinCount", "MaxCount"))
                .Add(new StronglyTypedAttributeArityRule<ArityAttribute>( "MinimumCount", "MaximumCount"))
                ;
            return rules;
        }

        public static RuleSetCommand SetFullCommandRules( RuleSetCommand rules)
        {

            rules.DescriptionRules
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<DescriptionAttribute, string>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<CommandAttribute, string>( "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<NameAttribute, string>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<CommandAttribute, string>( "Name"))
                .Add(new NameEndsWithRule("Command"))
                .Add(new IdentityRule<string>())
            ;

            rules.TreatUnmatchedTokensAsErrorsRules
                .Add(new StronglyTypedBooleanAttributeRule<TreatUnmatchedTokensAsErrorsAttribute>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<CommandAttribute, bool>( "TreatUnmatchedTokensAsErrors"));

            rules.IsHiddenRules
                .Add(new StronglyTypedBooleanAttributeRule<HiddenAttribute>())
                .Add(new StronglyTypedBooleanAttributeRule<CommandAttribute>( "IsHidden"));

            rules.AliasRules
                .Add(new StronglyTypedAttributeWithImpliedPropertyRule<AliasesAttribute, string[]>())
                .Add(new StronglyTypedAttributeWithPropertyValueRule<CommandAttribute, string[]>( "Aliases"))
               ;

            return rules;
        }

    }
}
