using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.CommandLine.ReflectionAppModel.Attributes;
using System.ComponentModel;
using System.Text;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    public class TypeWithEmptyMethod
    {
        public void EmptyMethod() { }
    }

    public class TypeWithMethodWithNameAttribute
    {
        [Name(TypeDescriptorMakerTests.Name)]
        public void Method() { }
    }

    public class TypeWithMethodWithDescriptionAttribute
    {
        [Description(TypeDescriptorMakerTests.Description)]
        public void Method() { }
    }

    public class TypeWithMethodWithNameInCommandAttribute
    {
        [Command(Name = TypeDescriptorMakerTests.Name)]
        public void Method() { }
    }

    public class TypeWithMethodWithDescriptionInCommandAttribute
    {
        [Command(Description = TypeDescriptorMakerTests.Description)]
        public void Method() { }
    }

    public class TypeWithMethodWithOneAliasAttribute
    {
        [Aliases(TypeDescriptorMakerTests.AliasAsStringSingle)]
        public void Method() { }
    }

    public class TypeWithMethodWithThreeAliasesInOneAttribute
    {
        [Aliases("a", "b", "c")]
        public void Method() { }
    }

    public class TypeWithMethodWithIsHiddenTrueInCommandAttribute
    {
        [Command(IsHidden = true)]
        public void Method() { }
    }

    public class TypeWithMethodWithIsHiddenFalseInCommandAttribute
    {
        [Command(IsHidden = false)]
        public void Method() { }
    }

    public class TypeWithMethodWithIsHiddenTrue
    {
        [Hidden(true)]
        public void Method() { }
    }

    public class TypeWithMethodWithIsHiddenFalse
    {
        [Hidden(false)]
        public void Method() { }
    }

    public class TypeWithMethodWithIsHiddenTrueAsImplied
    {
        [Hidden]
        public void Method() { }
    }


    public class TypeWithMethodWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute
    {
        [Command(TreatUnmatchedTokensAsErrors = true)]
        public void Method() { }
    }

    public class TypeWithMethodWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute
    {
        [Command(TreatUnmatchedTokensAsErrors = false)]
        public void Method() { }
    }

    public class TypeWithMethodWithTreatUnmatchedTokensAsErrorsTrue
    {
        [TreatUnmatchedTokensAsErrors(true)]
        public void Method() { }
    }

    public class TypeWithMethodWithTreatUnmatchedTokensAsErrorsFalse
    {
        [TreatUnmatchedTokensAsErrors(false)]
        public void Method() { }
    }

    public class TypeWithMethodWithTreatUnmatchedTokensAsErrorsTrueAsImplied
    {
        [TreatUnmatchedTokensAsErrors]
        public void Method() { }
    }

    public class TypeWithMethodWithOneArgumentByArgName
    {
        public void Method(string RedArg) { }
    }

    public class TypeWithMethodWithTwoArgumentByArgumentName
    {
        public void Method(string RedArgument, string BlueArgument) { }
    }

    public class TypeWithMethodWithOneArgumentByAttribute
    {

        public void Method([Argument] string Red) { }
    }

    public class TypeWithMethodWithTwoArgumentsByAttribute
    {
        public void Method([Argument] string Red, [Argument] string Blue) { }
    }

    public class TypeWithMethodWithOnlyOneParameter
    {
        public void Method(string East) { }
    }

    public class TypeWithMethodWithOneOptionByRemaining
    {
        public void Method([Argument] string Red, [Argument] string Blue, string East) { }
    }

    public class TypeWithMethodWithTwoOptionsByRemaining
    {
        public void Method([Argument] string Red, [Argument] string Blue, string East, string West) { }
    }

}
