using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Rules;

namespace System.CommandLine.GeneralAppModel
{
    public static class NamedAttributeRules
    {
        public static Strategy SetRules(this Strategy strategy)
        {
            SetFullSelectSymbolRules(strategy.SelectSymbolRules);
            SetFullCandidatesRules(strategy.GetCandidateRules);
            SetFullArgumentRules(strategy.ArgumentRules);
            SetFullCommandRules(strategy.CommandRules);
            SetFullOptionRules(strategy.OptionRules);
            SetFullOptionArgumentRules(strategy.OptionArgumentRules);
            return strategy;
        }

        public static RuleSetGetCandidatesRule SetFullCandidatesRules(this RuleSetGetCandidatesRule rules)
        {
            rules.Rules
                .Add(new DerivedFromRule())
                ;
            return rules;
        }

        public static RuleSetSelectSymbols SetFullSelectSymbolRules(this RuleSetSelectSymbols rules)
        {
            rules.Rules
               .Add(new AttributeRule("Command", SymbolType.Command))
               .Add(new NameEndsWithRule("Command", SymbolType.Command))
               .Add(new IsOfTypeRule<Type>(SymbolType.Command))
               .Add(new AttributeRule("Argument", SymbolType.Argument))
               .Add(new NameEndsWithRule("Argument", SymbolType.Argument))
               .Add(new NameEndsWithRule("Arg", SymbolType.Argument))
               .Add(new RemainingSymbolRule(SymbolType.Option))
            // TODO: .Add(new LabelRule<string>("ComplexUserType", SymbolType.Command))
            ;
            return rules;
        }

        public static RuleSetArgument SetFullArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new AttributeWithImpliedPropertyRule<string>("Description"))
                .Add(new AttributeWithPropertyValueRule<string>("Argument", "Description"))
                //.Add(new StronglyTypedAttributeWithImpliedPropertyRule<DescriptionAttribute, string>())
                //.Add(new StronglyTypedAttributeWithPropertyValueRule<ArgumentAttribute, string>("Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithImpliedPropertyRule<string>("Name"))
                .Add(new AttributeWithPropertyValueRule<string>("Argument", "Name"))
                .Add(new NameEndsWithRule("Arg"))
                .Add(new NameEndsWithRule("Argument"))
                .Add(new IdentityRule<string>())
                ;

            rules.DefaultValueRules
                .Add(new AttributeWithOptionalValueRule<object>("DefaultValue", "Value"))
                // DefaultValue on the Argument attribute is not trivial becaues we need to recognize when that is set in a generalized way"
                ;

            rules.RequiredRules
                .Add(new BooleanAttribute("Required"))
                .Add(new BooleanAttribute("Argument", "Required"))
            ;

            rules.IsHiddenRules
                .Add(new BooleanAttribute("Hidden"))
                .Add(new BooleanAttribute("Argument", "IsHidden"))
                ;

            rules.AliasRules
                .Add(new AttributeWithImpliedPropertyRule<string[]>("Aliases"))
                .Add(new AttributeWithPropertyValueRule<string[]>("Argument", "Aliases"))
                ;

            rules.AllowedValueRules
                .Add(new AttributeWithImpliedPropertyRule<object[]>("AllowedValues"))
                ;

            rules.ArityRules
                .Add(new RuleArity("Arity", "MinCount", "MaxCount"))
                .Add(new RuleArity("Arity", "MinimumCount", "MaximumCount"))
                ;

            return rules;
        }

        public static RuleSetOption SetFullOptionRules(this RuleSetOption rules)
        {

            rules.DescriptionRules
                .Add(new AttributeWithImpliedPropertyRule<string>("Description"))
                .Add(new AttributeWithPropertyValueRule<string>("Option", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithImpliedPropertyRule<string>("Name"))
                .Add(new AttributeWithPropertyValueRule<string>("Option", "Name"))
                .Add(new NameEndsWithRule("Option"))
                .Add(new IdentityRule<string>());

            rules.RequiredRules
                .Add(new BooleanAttribute("Required"))
                .Add(new BooleanAttribute("Option", "OptionRequired"))
             ;

            rules.IsHiddenRules
                .Add(new BooleanAttribute("Hidden"))
                .Add(new BooleanAttribute("Option", "IsHidden"))
                ;

            rules.AliasRules
                .Add(new AttributeWithImpliedPropertyRule<string[]>("Aliases"))
                .Add(new AttributeWithPropertyValueRule<string[]>("Option", "Aliases"))
                ;

            return rules;
        }

        public static RuleSetArgument SetFullOptionArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new AttributeWithPropertyValueRule<string>("Option", "ArgumentDescription", SymbolType.Argument))
                ;

            // The name is used for help
            rules.NameRules
                .Add(new AttributeWithPropertyValueRule<string>("Option", "ArgumentName", SymbolType.Argument))
                ;

            rules.RequiredRules
                .Add(new AttributeWithPropertyValueRule<bool>("Option", "ArgumentRequired", SymbolType.Option))
            ;

            rules.RequiredRules
                .Add(new AttributeWithPropertyValueRule<bool>("Option", "ArgumentIsHidden", SymbolType.Option))
            ;

            rules.DefaultValueRules
                .Add(new AttributeWithOptionalValueRule<object>("DefaultValue", "Value"))
                ;

            // Never hidden, no aliases

            rules.ArityRules
                .Add(new RuleArity("Arity", "MinCount", "MaxCount"))
                .Add(new RuleArity("Arity", "MinimumCount", "MaximumCount"))
                ;
            return rules;
        }

        public static RuleSetCommand SetFullCommandRules(this RuleSetCommand rules)
        {

            rules.DescriptionRules
                .Add(new AttributeWithImpliedPropertyRule<string>("Description"))
                .Add(new AttributeWithPropertyValueRule<string>("Command", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new AttributeWithImpliedPropertyRule<string>("Name"))
                .Add(new AttributeWithPropertyValueRule<string>("Command", "Name"))
                .Add(new NameEndsWithRule("Command"))
                .Add(new IdentityRule<string>())
            ;

            rules.TreatUnmatchedTokensAsErrorsRules
                .Add(new BooleanAttribute("TreatUnmatchedTokensAsErrors"))
                .Add(new AttributeWithPropertyValueRule<bool>("Command", "TreatUnmatchedTokensAsErrors"));

            rules.IsHiddenRules
                .Add(new BooleanAttribute("Hidden"))
                .Add(new BooleanAttribute("Command", "IsHidden"));

            rules.AliasRules
                .Add(new AttributeWithImpliedPropertyRule<string[]>("Aliases"))
                 .Add(new AttributeWithPropertyValueRule<string[]>("Command", "Aliases"))
               ;

            return rules;
        }

    }
}
