using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.CommandLine.ReflectionAppModel.Attributes;
using System.ComponentModel;
using System.Text;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    public class EmptyType
    {
    }

    [Name(TypeDescriptorMakerTests.Name)]
    public class TypeWithNameAttribute
    {
    }

    [Description(TypeDescriptorMakerTests.Description)]
    public class TypeWithDescriptionAttribute
    {
    }

    [Command(Name = TypeDescriptorMakerTests.Name)]
    public class TypeWithNameInCommandAttribute
    {
    }

    [Command(Description = TypeDescriptorMakerTests.Description)]
    public class TypeWithDescriptionInCommandAttribute
    {
    }

    [Aliases(TypeDescriptorMakerTests.AliasAsStringSingle)]
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

    [Hidden( true)]
    public class TypeWithIsHiddenTrue
    {
    }

    [Hidden( false)]
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
        public string RedArg { get; set; }
    }

    public class TypeWithTwoArgumentByArgumentName
    {
        public string RedArgument { get; set; }
        public string BlueArgument { get; set; }
    }

    public class TypeWithOneArgumentByAttribute
    {
        [Argument]
        public string Red { get; set; }
    }

    public class TypeWithTwoArgumentsByAttribute
    {
        [Argument]
        public string Red { get; set; }
        [Argument]
        public string Blue { get; set; }
    }

    public class TypeWithOneCommandByDerivedType
    {
        public class A : TypeWithOneCommandByDerivedType { }
    }

    public class TypeWithTwoCommandsByDerivedType
    {
        public class A : TypeWithTwoCommandsByDerivedType { }
        public class B : TypeWithTwoCommandsByDerivedType { }
    }

    public class TypeWithOnlyOneProperty
    {
        public string East { get; set; }
    }

    public class TypeWithOneOptionByRemaining
    {
        public string RedArgument { get; set; }
        public string East { get; set; }
    }

    public class TypeWithTwoOptionsByRemaining
    {
        [Argument]
        public string RedArgument { get; set; }
        [Argument]
        public string Blue { get; set; }
        public string East { get; set; }
        public string West { get; set; }

    }

}
