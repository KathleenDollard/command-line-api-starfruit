using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Rules;

namespace System.CommandLine.NamedAttributeRules
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
               .Add(new NamedAttributeRule("Command", SymbolType.Command))
               .Add(new NameEndsWithRule("Command", SymbolType.Command))
               .Add(new IsOfTypeRule<Type>(SymbolType.Command))
               .Add(new NamedAttributeRule("Argument", SymbolType.Argument))
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
                .Add(new NamedAttributeWithImpliedPropertyRule<string>("Description"))
                .Add(new NamedAttributeWithPropertyValueRule<string>("Argument", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeWithImpliedPropertyRule<string>("Name"))
                .Add(new NamedAttributeWithPropertyValueRule<string>("Argument", "Name"))
                .Add(new NameEndsWithRule("Arg"))
                .Add(new NameEndsWithRule("Argument"))
                .Add(new IdentityRule<string>())
                ;

            rules.DefaultValueRules
                .Add(new NamedAttributeWithOptionalValueRule<object>("DefaultValue", "Value"))
                // DefaultValue on the Argument attribute is not trivial becaues we need to recognize when that is set in a generalized way"
                ;

            rules.RequiredRules
                .Add(new NamedBooleanAttributeRule("Required"))
                .Add(new NamedBooleanAttributeRule("Argument", "Required"))
            ;

            rules.IsHiddenRules
                .Add(new NamedBooleanAttributeRule("Hidden"))
                .Add(new NamedBooleanAttributeRule("Argument", "IsHidden"))
                ;

            rules.AliasRules
                .Add(new NamedAttributeWithImpliedPropertyRule<string[]>("Aliases"))
                .Add(new NamedAttributeWithPropertyValueRule<string[]>("Argument", "Aliases"))
                ;

            rules.AllowedValueRules
                .Add(new NamedAttributeWithImpliedPropertyRule<object[]>("AllowedValues"))
                ;

            rules.ArityRules
                .Add(new NamedArityAttributeRule("Arity", "MinCount", "MaxCount"))
                .Add(new NamedArityAttributeRule("Arity", "MinimumCount", "MaximumCount"))
                ;

            return rules;
        }

        public static RuleSetOption SetFullOptionRules(this RuleSetOption rules)
        {

            rules.DescriptionRules
                .Add(new NamedAttributeWithImpliedPropertyRule<string>("Description"))
                .Add(new NamedAttributeWithPropertyValueRule<string>("Option", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeWithImpliedPropertyRule<string>("Name"))
                .Add(new NamedAttributeWithPropertyValueRule<string>("Option", "Name"))
                .Add(new NameEndsWithRule("Option"))
                .Add(new IdentityRule<string>());

            rules.RequiredRules
                .Add(new NamedBooleanAttributeRule("Required"))
                .Add(new NamedBooleanAttributeRule("Option", "OptionRequired"))
             ;

            rules.IsHiddenRules
                .Add(new NamedBooleanAttributeRule("Hidden"))
                .Add(new NamedBooleanAttributeRule("Option", "IsHidden"))
                ;

            rules.AliasRules
                .Add(new NamedAttributeWithImpliedPropertyRule<string[]>("Aliases"))
                .Add(new NamedAttributeWithPropertyValueRule<string[]>("Option", "Aliases"))
                ;

            return rules;
        }

        public static RuleSetArgument SetFullOptionArgumentRules(this RuleSetArgument rules)
        {
            rules.DescriptionRules
                .Add(new NamedAttributeWithPropertyValueRule<string>("Option", "ArgumentDescription", SymbolType.Argument))
                ;

            // The name is used for help
            rules.NameRules
                .Add(new NamedAttributeWithPropertyValueRule<string>("Option", "ArgumentName", SymbolType.Argument))
                ;

            rules.RequiredRules
                .Add(new NamedAttributeWithPropertyValueRule<bool>("Option", "ArgumentRequired", SymbolType.Option))
            ;

            rules.RequiredRules
                .Add(new NamedAttributeWithPropertyValueRule<bool>("Option", "ArgumentIsHidden", SymbolType.Option))
            ;

            rules.DefaultValueRules
                .Add(new NamedAttributeWithOptionalValueRule<object>("DefaultValue", "Value"))
                ;

            // Never hidden, no aliases

            rules.ArityRules
                .Add(new NamedArityAttributeRule("Arity", "MinCount", "MaxCount"))
                .Add(new NamedArityAttributeRule("Arity", "MinimumCount", "MaximumCount"))
                ;
            return rules;
        }

        public static RuleSetCommand SetFullCommandRules(this RuleSetCommand rules)
        {

            rules.DescriptionRules
                .Add(new NamedAttributeWithImpliedPropertyRule<string>("Description"))
                .Add(new NamedAttributeWithPropertyValueRule<string>("Command", "Description"))
                // TODO: .Add(new LabelRule<string>("XmlDocComments") )
                ;
            rules.NameRules
                .Add(new NamedAttributeWithImpliedPropertyRule<string>("Name"))
                .Add(new NamedAttributeWithPropertyValueRule<string>("Command", "Name"))
                .Add(new NameEndsWithRule("Command"))
                .Add(new IdentityRule<string>())
            ;

            rules.TreatUnmatchedTokensAsErrorsRules
                .Add(new NamedBooleanAttributeRule("TreatUnmatchedTokensAsErrors"))
                .Add(new NamedAttributeWithPropertyValueRule<bool>("Command", "TreatUnmatchedTokensAsErrors"));

            rules.IsHiddenRules
                .Add(new NamedBooleanAttributeRule("Hidden"))
                .Add(new NamedBooleanAttributeRule("Command", "IsHidden"));

            rules.AliasRules
                .Add(new NamedAttributeWithImpliedPropertyRule<string[]>("Aliases"))
                 .Add(new NamedAttributeWithPropertyValueRule<string[]>("Command", "Aliases"))
               ;

            return rules;
        }

    }
}
