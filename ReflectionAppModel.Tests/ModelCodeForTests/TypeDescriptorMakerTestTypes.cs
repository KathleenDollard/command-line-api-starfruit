using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Tests;
using System.Text;
using constants = System.CommandLine.GeneralAppModel.Tests.TypeDescriptorMakerTests;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests.TypedAttributes
{
    public class EmptyType
    {
    }

    [Name(constants.Name)]
    public class TypeWithNameAttribute
    {
    }

    [Description(constants.Description)]
    public class TypeWithDescriptionAttribute
    {
    }

    [Command(Name = constants.Name)]
    public class TypeWithNameInCommandAttribute
    {
    }

    [Command(Description = constants.Description)]
    public class TypeWithDescriptionInCommandAttribute
    {
    }

    [Aliases(constants.AliasAsStringSingle)]
    public class TypeWithOneAliasAttribute
    {
    }

    [Aliases("a", "b", "c")]
    public class TypeWithThreeAliasesInOneAttribute
    {
    }

    [Command(IsHidden = true)]
    public class TypeWithIsHiddenTrueInCommandAttribute
    {
    }

    [Command(IsHidden = false)]
    public class TypeWithIsHiddenFalseInCommandAttribute
    {
    }

    [Hidden(true)]
    public class TypeWithIsHiddenTrue
    {
    }

    [Hidden(false)]
    public class TypeWithIsHiddenFalse
    {
    }

    [Hidden]
    public class TypeWithIsHiddenTrueAsImplied
    {
    }

    [Command(TreatUnmatchedTokensAsErrors = true)]
    public class TypeWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute
    {
    }

    [Command(TreatUnmatchedTokensAsErrors = false)]
    public class TypeWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute
    {
    }

    [TreatUnmatchedTokensAsErrors(true)]
    public class TypeWithTreatUnmatchedTokensAsErrorsTrue
    {
    }

    [TreatUnmatchedTokensAsErrors(false)]
    public class TypeWithTreatUnmatchedTokensAsErrorsFalse
    {
    }

    [TreatUnmatchedTokensAsErrors]
    public class TypeWithTreatUnmatchedTokensAsErrorsTrueAsImplied
    {
    }

    public class TypeWithOneArgumentByArgName
    {
        public string? RedArg { get; set; }
    }

    public class TypeWithTwoArgumentByArgumentName
    {
        public string? RedArgument { get; set; }
        public string? BlueArgument { get; set; }
    }

    public class TypeWithOneArgumentByAttribute
    {
        [Argument]
        public string? Red { get; set; }
    }

    public class TypeWithTwoArgumentsByAttribute
    {
        [Argument]
        public string? Red { get; set; }
        [Argument]
        public string? Blue { get; set; }
    }

    public class TypeWithOneCommandByDerivedType
    {
        public class A : TypeWithOneCommandByDerivedType { }
    }

    public class TypeWithTwoCommandsByDerivedType
    {
        public class A : TypeWithTwoCommandsByDerivedType
        {
            public bool Allow { get; set; }
        }
        public class B : TypeWithTwoCommandsByDerivedType { }
    }

    public class TypeWithOnlyOneProperty
    {
        public string? East { get; set; }
    }

    public class TypeWithOneOptionByRemaining
    {
        public string? RedArg { get; set; }
        public string? East { get; set; }
    }

    public class TypeWithTwoOptionsByRemaining
    {
        [Argument]
        public string? RedArg { get; set; }
        [Argument]
        public string? Blue { get; set; }
        public string? East { get; set; }
        public string? West { get; set; }

    }

    public class TypeWithInvokeMethod
    {
        public void Invoke() { }
        public void Blah() { }
    }

    public class TypeWithTwoInvokeMethods
    {
        public void Invoke() { }

        public void Invoke(string p1, int p2) { }
    }

  
    // Option test classes

    public class PropertyOptionWithName
    {
        public string? George { get; set; }
    }

    public class PropertyOptionWithNameAttribute
    {
        [Name(constants.Name)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithNameInOptionAttribute
    {
        [Option(Name = constants.Name)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithDescriptionAttribute
    {
        [Description(constants.Description)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithDescriptionInOptionAttribute
    {
        [Option(Description = constants.Description)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithOneAliasAttribute
    {
        [Aliases(constants.AliasAsStringSingle)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithThreeAliasesInOneAttribute
    {
        [Aliases("a", "b", "c")]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithIsHiddenTrueInOptionAttribute
    {
        [Option(IsHidden = true)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithIsHiddenFalseInOptionAttribute
    {
        [Option(IsHidden = false)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithIsHiddenTrue
    {
        [Hidden(true)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithIsHiddenFalse
    {
        [Hidden(false)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithIsHiddenTrueAsImplied
    {
        [Hidden]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithRequiredTrueInOptionAttribute
    {
        [Option(OptionRequired = true)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithRequiredFalseInOptionAttribute
    {
        [Option(OptionRequired = false)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithRequiredTrue
    {
        [Required(true)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithRequiredFalse
    {
        [Required(false)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionWithRequiredTrueAsImplied
    {
        [Required]
        public string? Prop { get; set; }
    }

    public class PropertyOptionArgumentWithNoDefaultValue
    {
        public string? Prop { get; set; }
    }

    public class PropertyOptionArgumentWithStringDefaultValue
    {
        [DefaultValue(constants.DefaultValueString)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionArgumentWithIntegerDefaultValue
    {
        [DefaultValue(constants.DefaultValueInt)]
        public string? Prop { get; set; }
    }

    public class PropertyOptionArgumentForIntegerType
    {
        public int Prop { get; set; }
    }
    public class PropertiesThatArePublicAndPrivate
    {
        public string? First { get; set; }
        private string? Second { get; set; }
    }




    // Argument
    public class PropertyArgumentWithName
    {
        public string? GeorgeArg { get; set; }
    }

    public class PropertyArgumentWithNameAttribute
    {
        [Name(constants.Name)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithDescriptionAttribute
    {
        [Description(constants.Description)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithNameInArgumentAttribute
    {
        [Argument(Name = constants.Name)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithDescriptionInArgumentAttribute
    {
        [Argument(Description = constants.Description)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithOneAliasAttribute
    {
        [Aliases(constants.AliasAsStringSingle)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithThreeAliasesInOneAttribute
    {
        [Aliases(constants.AliasAsStringMultiple)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithOneAllowedValueAttribute
    {
        [AllowedValues(constants.AllowedValuesAsIntFirst)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithThreeAllowedValuesInOneAttribute
    {
        [AllowedValues(constants.AllowedValuesAsIntFirst,
                                          constants.AllowedValuesAsIntSecond,
                                          constants.AllowedValuesAsIntThird)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithIsHiddenTrueInArgumentAttribute
    {
        [Argument(IsHidden = true)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithIsHiddenFalseInArgumentAttribute
    {
        [Argument(IsHidden = false)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithIsHiddenTrue
    {
        [Hidden(true)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithIsHiddenFalse
    {
        [Hidden(false)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithIsHiddenTrueAsImplied
    {
        [Hidden]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithRequiredTrueInArgumentAttribute
    {
        [Argument(Required = true)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithRequiredFalseInArgumentAttribute
    {
        [Argument(Required = false)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithRequiredTrue
    {
        [Required(true)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithRequiredFalse
    {
        [Required(false)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithRequiredTrueAsImplied
    {
        [Required]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithNoArity
    {
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithArityLowerBoundOnly
    {
        [Arity(minimumCount: 2)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithArityBothBounds
    {
        [Arity(minimumCount: 2, maximumCount: 3)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithNoDefaultValue
    {
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithStringDefaultValue
    {
        [DefaultValue(constants.DefaultValueString)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentWithIntegerDefaultValue
    {
        [DefaultValue(constants.DefaultValueInt)]
        public string? PropArg { get; set; }
    }

    public class PropertyArgumentForIntegerType
    {
        public int PropArg { get; set; }
    }

    public class PropertyOnBaseOnlyOnParentCommand
    {
        public int PropArg { get; set; }
        public class Derived
        { }
    }
}
