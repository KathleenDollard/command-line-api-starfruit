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

    [Name(NewDescriptorMakerTests.Name)]
    public class TypeWithNameAttribute
    {
    }

    [Description(NewDescriptorMakerTests.Description)]
    public class TypeWithDescriptionAttribute
    {
    }

    [Command(Name = NewDescriptorMakerTests.Name)]
    public class TypeWithNameInCommandAttribute
    {
    }

    [Command(Description = NewDescriptorMakerTests.Description)]
    public class TypeWithDescriptionInCommandAttribute
    {
    }

    [Aliases(NewDescriptorMakerTests.AliasAsStringSingle)]
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
}
