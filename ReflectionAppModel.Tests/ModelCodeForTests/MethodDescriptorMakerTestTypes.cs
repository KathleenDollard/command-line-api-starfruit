using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.CommandLine.ReflectionAppModel.Attributes;
using System.ComponentModel;
using System.Text;
using constant = System.CommandLine.GeneralAppModel.Tests.TypeDescriptorMakerTests;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    public class MethodEmptyMethod
    {
        public void EmptyMethod() { }
    }

    public class MethodWithNameAttribute
    {
        [Name(constant.Name)]
        public void Method() { }
    }

    public class MethodWithDescriptionAttribute
    {
        [Description(constant.Description)]
        public void Method() { }
    }

    public class MethodWithNameInCommandAttribute
    {
        [Command(Name = constant.Name)]
        public void Method() { }
    }

    public class MethodWithDescriptionInCommandAttribute
    {
        [Command(Description = constant.Description)]
        public void Method() { }
    }

    public class MethodWithOneAliasAttribute
    {
        [Aliases(constant.AliasAsStringSingle)]
        public void Method() { }
    }

    public class MethodWithThreeAliasesInOneAttribute
    {
        [Aliases("a", "b", "c")]
        public void Method() { }
    }

    public class MethodWithIsHiddenTrueInCommandAttribute
    {
        [Command(IsHidden = true)]
        public void Method() { }
    }

    public class MethodWithIsHiddenFalseInCommandAttribute
    {
        [Command(IsHidden = false)]
        public void Method() { }
    }

    public class MethodWithIsHiddenTrue
    {
        [Hidden(true)]
        public void Method() { }
    }

    public class MethodWithIsHiddenFalse
    {
        [Hidden(false)]
        public void Method() { }
    }

    public class MethodWithIsHiddenTrueAsImplied
    {
        [Hidden]
        public void Method() { }
    }

    public class MethodWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute
    {
        [Command(TreatUnmatchedTokensAsErrors = true)]
        public void Method() { }
    }

    public class MethodWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute
    {
        [Command(TreatUnmatchedTokensAsErrors = false)]
        public void Method() { }
    }

    public class MethodWithTreatUnmatchedTokensAsErrorsTrue
    {
        [TreatUnmatchedTokensAsErrors(true)]
        public void Method() { }
    }

    public class MethodWithTreatUnmatchedTokensAsErrorsFalse
    {
        [TreatUnmatchedTokensAsErrors(false)]
        public void Method() { }
    }

    public class MethodWithTreatUnmatchedTokensAsErrorsTrueAsImplied
    {
        [TreatUnmatchedTokensAsErrors]
        public void Method() { }
    }

    public class MethodWithOneArgumentByArgName
    {
        public void Method(string RedArg) { }
    }

    public class MethodWithTwoArgumentByArgumentName
    {
        public void Method(string RedArgument, string BlueArgument) { }
    }

    public class MethodWithOneArgumentByAttribute
    {

        public void Method([Argument] string Red) { }
    }

    public class MethodWithTwoArgumentsByAttribute
    {
        public void Method([Argument] string Red, [Argument] string Blue) { }
    }

    public class MethodWithOnlyOneParameter
    {
        public void Method(string East) { }
    }

    public class MethodWithOneOptionByRemaining
    {
        public void Method([Argument] string Red, [Argument] string Blue, string East) { }
    }

    public class MethodWithTwoOptionsByRemaining
    {
        public void Method([Argument] string Red, [Argument] string Blue, string East, string West) { }
    }

    
    
    // Option test classes

    public class ParameterOptionWithName
    {
        public void Method(string George) { }
    }

    public class ParameterOptionWithNameAttribute
    {
        
        public void Method([Name(constant.Name)]string Param) { }
    }

    public class ParameterOptionWithNameInOptionAttribute
    {
        public void Method( [Option(Name = constant.Name)]string Param) { }
    }

    public class ParameterOptionWithDescriptionAttribute
    {
        public void Method([Description(constant.Description)]string Param) { }
    }

    public class ParameterOptionWithDescriptionInOptionAttribute
    {
        public void Method([Option(Description = constant.Description)]string Param) { }
    }

    public class ParameterOptionWithOneAliasAttribute
    {
        public void Method( [Aliases(constant.AliasAsStringSingle)]string Param) { }
    }

    public class ParameterOptionWithThreeAliasesInOneAttribute
    {
        public void Method(  [Aliases(constant.AliasAsStringMultiple)]string Param) { }
    }

    public class ParameterOptionWithOneAllowedValueAttribute
    {
        public void Method([AllowedValues(constant.AllowedValuesAsIntFirst)] string Param) { }
    }

    public class ParameterOptionWithThreeAllowedVauesInOneAttribute
    {
        public void Method([AllowedValues(constant.AllowedValuesAsIntFirst,
                                          constant.AllowedValuesAsIntSecond,
                                          constant.AllowedValuesAsIntThird)] string Param) { }
    }

    public class ParameterOptionWithIsHiddenTrueInOptionAttribute
    {
        public void Method([Option(IsHidden = true)]string Param) { }
    }

    public class ParameterOptionWithIsHiddenFalseInOptionAttribute
    {
        public void Method( [Option(IsHidden = false)]string Param) { }
    }

    public class ParameterOptionWithIsHiddenTrue
    {
        public void Method([Hidden(true)]string Param) { }
    }

    public class ParameterOptionWithIsHiddenFalse
    {
        public void Method( [Hidden(false)]string Param) { }
    }

    public class ParameterOptionWithIsHiddenTrueAsImplied
    {
        public void Method([Hidden]string Param) { }
    }

    public class ParameterOptionWithRequiredTrueInOptionAttribute
    {
        public void Method([Option(OptionRequired = true)]string Param) { }
    }

    public class ParameterOptionWithRequiredFalseInOptionAttribute
    {
        public void Method([Option(OptionRequired = false)]string Param) { }
    }

    public class ParameterOptionWithRequiredTrue
    {
        public void Method( [Required(true)]string Param) { }
    }

    public class ParameterOptionWithRequiredFalse
    {
        public void Method( [Required(false)]string Param) { }
    }

    public class ParameterOptionWithRequiredTrueAsImplied
    {
        public void Method([Required]string Param) { }
    }

    public class ParameterOptionArgumentWithNoDefaultValue
    {
        public void Method(string Param) { }
    }

    public class ParameterOptionArgumentWithStringDefaultValue
    {
        public void Method( [DefaultValue(constant.DefaultValueString)]string Param) { }
    }

    public class ParameterOptionArgumentWithIntegerDefaultValue
    {
        public void Method([DefaultValue(constant.DefaultValueInt)]string Param) { }
    }

    public class ParameterOptionArgumentForIntegerType
    {
        public void Method(int Param) { }
    }




    public class ParameterArgumentWithName
    {
        public void Method(string GeorgeArg) { }
    }

    public class ParameterArgumentWithNameAttribute
    {
        public void Method([Name(constant.Name)]string ParamArg) { }
    }

    public class ParameterArgumentWithDescriptionAttribute
    {
        public void Method( [Description(constant.Description)]string ParamArg) { }
    }

    public class ParameterArgumentWithNameInArgumentAttribute
    {
        public void Method( [Argument(Name = constant.Name)]string ParamArg) { }
    }

    public class ParameterArgumentWithDescriptionInArgumentAttribute
    {
        public void Method( [Argument(Description = constant.Description)]string ParamArg) { }
    }

    public class ParameterArgumentWithOneAliasAttribute
    {
        public void Method( [Aliases(constant.AliasAsStringSingle)]string ParamArg) { }
    }

    public class ParameterArgumentWithThreeAliasesInOneAttribute
    {
        public void Method([Aliases("a", "b", "c")]string ParamArg) { }
    }

    public class ParameterArgumentWithOneAllowedValueAttribute
    {
        public void Method([AllowedValues(constant.AllowedValuesAsIntFirst)] string ParamArg) { }
    }

    public class ParameterArgumentWithThreeAllowedVauesInOneAttribute
    {
        public void Method([AllowedValues(constant.AllowedValuesAsIntFirst,
                                          constant.AllowedValuesAsIntSecond,
                                          constant.AllowedValuesAsIntThird)] string ParamArg)
        { }
    }

    public class ParameterArgumentWithIsHiddenTrueInArgumentAttribute
    {
        public void Method([Argument(IsHidden = true)]string ParamArg) { }
    }

    public class ParameterArgumentWithIsHiddenFalseInArgumentAttribute
    {
        public void Method( [Argument(IsHidden = false)]string ParamArg) { }
    }

    public class ParameterArgumentWithIsHiddenTrue
    {
        public void Method([Hidden(true)]string ParamArg) { }
    }

    public class ParameterArgumentWithIsHiddenFalse
    {
        public void Method( [Hidden(false)]string ParamArg) { }
    }

    public class ParameterArgumentWithIsHiddenTrueAsImplied
    {
        public void Method([Hidden]string ParamArg) { }
    }

    public class ParameterArgumentWithRequiredTrueInArgumentAttribute
    {
        public void Method([Argument(Required = true)]string ParamArg) { }
    }

    public class ParameterArgumentWithRequiredFalseInArgumentAttribute
    {
        public void Method([Argument(Required = false)]string ParamArg) { }
    }

    public class ParameterArgumentWithRequiredTrue
    {
        public void Method([Required(true)]string ParamArg) { }
    }

    public class ParameterArgumentWithRequiredFalse
    {
        public void Method([Required(false)]string ParamArg) { }
    }

    public class ParameterArgumentWithRequiredTrueAsImplied
    {
        public void Method(  [Required]string ParamArg) { }
    }

    public class ParameterArgumentWithNoArity
    {
        public void Method(string ParamArg) { }
    }

    public class ParameterArgumentWithArityLowerBoundOnly
    {
        public void Method( [Arity(minimumCount:  2)]string ParamArg) { }
    }

    public class ParameterArgumentWithArityBothBounds
    {
        public void Method([Arity(minimumCount :2, maximumCount : 3)]string ParamArg) { }
    }

    public class ParameterArgumentWithNoDefaultValue
    {
        public void Method(string ParamArg) { }
    }

    public class ParameterArgumentWithStringDefaultValue
    {
        public void Method([DefaultValue(constant.DefaultValueString)]string ParamArg) { }
    }

    public class ParameterArgumentWithIntegerDefaultValue
    {
        public void Method([DefaultValue(constant.DefaultValueInt)]string ParamArg) { }
    }

    public class ParameterArgumentForIntegerType
    {
        public void Method(int ParamArg) { }
    }

}
