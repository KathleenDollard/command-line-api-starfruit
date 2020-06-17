using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests.Maker;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests.TypedAttributes;
using System.Linq;
using Xunit;
using constant = System.CommandLine.GeneralAppModel.Tests.TypeDescriptorMakerTests;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class MethodDescriptorMakerTests
    {

        private readonly Strategy fullStrategy;
        private readonly Strategy standardStrategy;
        private const string full = "Full";
        private const string standard = "Standard";

        public MethodDescriptorMakerTests()
        {
            fullStrategy = new Strategy().SetFullRules();
            standardStrategy = new Strategy().SetStandardRules();
        }

        #region Command tests
        [Theory]
        [InlineData(full, typeof(MethodEmptyMethod), nameof(MethodEmptyMethod.EmptyMethod), "")]
        [InlineData(full, typeof(MethodWithNameAttribute), constant.Name, "")]
        [InlineData(full, typeof(MethodWithNameInCommandAttribute), constant.Name, "")]
        [InlineData(full, typeof(MethodWithDescriptionAttribute), constant.TestMethodName, constant.Description)]
        [InlineData(full, typeof(MethodWithDescriptionInCommandAttribute), constant.TestMethodName, constant.Description)]
        [InlineData(standard, typeof(MethodEmptyMethod), nameof(MethodEmptyMethod.EmptyMethod), "")]
        [InlineData(standard, typeof(MethodWithNameInCommandAttribute), constant.Name, "")]
        [InlineData(standard, typeof(MethodWithDescriptionInCommandAttribute), constant.TestMethodName, constant.Description)]
        public void NameAndDescriptionFromType(string useStrategy, Type typeToTest, string name, string description)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy , method);

            descriptor.Should().HaveName(name)
                    .And.HaveDescription(description);
        }


        [Theory]
        [InlineData(full, typeof(MethodWithOneAliasAttribute), constant.AliasAsStringSingle)]
        [InlineData(full, typeof(MethodWithThreeAliasesInOneAttribute), constant.AliasAsStringMultiple)]
        public void AliasesFromType(string useStrategy, Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(',').Select(s => s.Trim()).ToArray();

            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(full, typeof(MethodWithIsHiddenTrueInCommandAttribute), true)]
        [InlineData(full, typeof(MethodWithIsHiddenFalseInCommandAttribute), false)]
        [InlineData(full, typeof(MethodWithIsHiddenTrue), true)]
        [InlineData(full, typeof(MethodWithIsHiddenFalse), false)]
        [InlineData(full, typeof(MethodWithIsHiddenTrueAsImplied), true)]
        [InlineData(standard, typeof(MethodWithIsHiddenTrueInCommandAttribute), true)]
        [InlineData(standard, typeof(MethodWithIsHiddenFalseInCommandAttribute), false)]
        public void CommandIsHiddenFromType(string useStrategy, Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(MethodWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute), true)]
        [InlineData(full, typeof(MethodWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute), false)]
        [InlineData(full, typeof(MethodWithTreatUnmatchedTokensAsErrorsTrue), true)]
        [InlineData(full, typeof(MethodWithTreatUnmatchedTokensAsErrorsFalse), false)]
        [InlineData(full, typeof(MethodWithTreatUnmatchedTokensAsErrorsTrueAsImplied), true)]
        [InlineData(standard, typeof(MethodWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute), true)]
        [InlineData(standard, typeof(MethodWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute), false)]
        public void CommandTreatUnmatchedTokensAsErrorsFromType(string useStrategy, Type typeToTest, bool treatUnmatchedTokensAsErrors)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Should().HaveTreatUnmatchedTokensAsErrors(treatUnmatchedTokensAsErrors);
        }

        [Theory]
        [InlineData(full, typeof(MethodWithOneArgumentByArgName), constant.ArgumentName)]
        [InlineData(full, typeof(MethodWithTwoArgumentByArgumentName), constant.ArgumentName, constant.ArgumentName2)]
        [InlineData(full, typeof(MethodWithOneArgumentByAttribute), constant.ArgumentName)]
        [InlineData(full, typeof(MethodWithTwoArgumentsByAttribute), constant.ArgumentName, constant.ArgumentName2)]
        [InlineData(standard, typeof(MethodWithOneArgumentByArgName), constant.ArgumentName)]
        [InlineData(standard, typeof(MethodWithOneArgumentByAttribute), constant.ArgumentName)]
        [InlineData(standard, typeof(MethodWithTwoArgumentsByAttribute), constant.ArgumentName, constant.ArgumentName2)]
        public void CommandWithArguments(string useStrategy, Type typeToTest, params string[] argNames)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Should().HaveArgumentsNamed(argNames);
        }

        [Theory]
        [InlineData(full, typeof(MethodWithOnlyOneParameter), "--" + constant.OptionName)]
        [InlineData(full, typeof(MethodWithOneOptionByRemaining), "--" + constant.OptionName)]
        [InlineData(full, typeof(MethodWithTwoOptionsByRemaining), "--" + constant.OptionName, "--" + constant.OptionName2)]
        [InlineData(standard, typeof(MethodWithOnlyOneParameter), "--" + constant.OptionName)]
        [InlineData(standard, typeof(MethodWithOneOptionByRemaining), "--" + constant.OptionName)]
        [InlineData(standard, typeof(MethodWithTwoOptionsByRemaining), "--" + constant.OptionName, "--" + constant.OptionName2)]
        public void CommandWithSubOptions(string useStrategy, Type typeToTest, params string[] argNames)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Should().HaveOptionsNamed(argNames);
        }

        #endregion

        #region Option tests

        [Theory]
        [InlineData(full, typeof(ParameterOptionWithName), "--" + constant.Name, "")]
        [InlineData(full, typeof(ParameterOptionWithNameAttribute), "--" + constant.Name, "")]
        [InlineData(full, typeof(ParameterOptionWithNameInOptionAttribute), "--" + constant.Name, "")]
        [InlineData(full, typeof(ParameterOptionWithDescriptionAttribute), "--" + constant.ParameterOptionName, constant.Description)]
        [InlineData(full, typeof(ParameterOptionWithDescriptionInOptionAttribute), "--" + constant.ParameterOptionName, constant.Description)]
        [InlineData(standard, typeof(ParameterOptionWithName), "--" + constant.Name, "")]
        [InlineData(standard, typeof(ParameterOptionWithNameInOptionAttribute), "--" + constant.Name, "")]
        [InlineData(standard, typeof(ParameterOptionWithDescriptionInOptionAttribute), "--" + constant.ParameterOptionName, constant.Description)]
        public void OptionNameAndDescriptionFromParameter(string useStrategy, Type typeToTest, string name, string description)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Options.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(full, typeof(ParameterOptionWithOneAliasAttribute), constant.OptionAliasAsStringSingle)]
        [InlineData(full, typeof(ParameterOptionWithThreeAliasesInOneAttribute), constant.OptionAliasAsStringMultiple)]
        public void OptionAliasesFromParameter(string useStrategy, Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(',').Select(s => s.Trim()).ToArray();

            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Options.First()
                    .Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(full, typeof(ParameterOptionWithIsHiddenTrueInOptionAttribute), true)]
        [InlineData(full, typeof(ParameterOptionWithIsHiddenFalseInOptionAttribute), false)]
        [InlineData(full, typeof(ParameterOptionWithIsHiddenTrue), true)]
        [InlineData(full, typeof(ParameterOptionWithIsHiddenFalse), false)]
        [InlineData(full, typeof(ParameterOptionWithIsHiddenTrueAsImplied), true)]
        [InlineData(standard, typeof(ParameterOptionWithIsHiddenTrueInOptionAttribute), true)]
        [InlineData(standard, typeof(ParameterOptionWithIsHiddenFalseInOptionAttribute), false)]
        public void OptionIsHiddenFromParameter(string useStrategy, Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Options.First()
                    .Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(ParameterOptionWithRequiredTrueInOptionAttribute), true)]
        [InlineData(full, typeof(ParameterOptionWithRequiredFalseInOptionAttribute), false)]
        [InlineData(full, typeof(ParameterOptionWithRequiredTrue), true)]
        [InlineData(full, typeof(ParameterOptionWithRequiredFalse), false)]
        [InlineData(full, typeof(ParameterOptionWithRequiredTrueAsImplied), true)]
        [InlineData(standard, typeof(ParameterOptionWithRequiredTrueInOptionAttribute), true)]
        [InlineData(standard, typeof(ParameterOptionWithRequiredFalseInOptionAttribute), false)]
        public void OptionRequiredFromParameter(string useStrategy, Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Options.First()
                    .Should().HaveRequired(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(ParameterOptionArgumentWithNoDefaultValue), false, null)]
        [InlineData(full, typeof(ParameterOptionArgumentWithStringDefaultValue), true, constant.DefaultValueString)]
        [InlineData(full, typeof(ParameterOptionArgumentWithIntegerDefaultValue), true, constant.DefaultValueInt)]
        [InlineData(standard, typeof(ParameterOptionArgumentWithNoDefaultValue), false, null)]
        [InlineData(standard, typeof(ParameterOptionArgumentWithStringDefaultValue), true, constant.DefaultValueString)]
        [InlineData(standard, typeof(ParameterOptionArgumentWithIntegerDefaultValue), true, constant.DefaultValueInt)]
        public void OptionDefaultValueFromParameter(string useStrategy, Type typeToTest, bool isSet, object value)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Options.First().Arguments.First()
                    .Should().HaveDefaultValue(isSet, value);
        }

        [Theory]
        [InlineData(full, typeof(ParameterOptionWithName), typeof(string))]
        [InlineData(full, typeof(ParameterOptionArgumentForIntegerType), typeof(int))]
        [InlineData(standard, typeof(ParameterOptionWithName), typeof(string))]
        [InlineData(standard, typeof(ParameterOptionArgumentForIntegerType), typeof(int))]
        public void OptionWithArguments(string useStrategy, Type typeToTest, Type argType)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Options.First().Arguments.First()
                    .Should().HaveArgumentType(argType);
        }

        #endregion

        #region Argument Tests

        [Theory]
        [InlineData(full, typeof(ParameterArgumentWithName), constant.Name, "")]
        [InlineData(full, typeof(ParameterArgumentWithNameAttribute), constant.Name, "")]
        [InlineData(full, typeof(ParameterArgumentWithNameInArgumentAttribute), constant.Name, "")]
        [InlineData(full, typeof(ParameterArgumentWithDescriptionAttribute), constant.ParameterArgName, constant.Description)]
        [InlineData(full, typeof(ParameterArgumentWithDescriptionInArgumentAttribute), constant.ParameterArgName, constant.Description)]
        [InlineData(standard, typeof(ParameterArgumentWithName), constant.Name, "")]
        [InlineData(standard, typeof(ParameterArgumentWithNameInArgumentAttribute), constant.Name, "")]
        [InlineData(standard, typeof(ParameterArgumentWithDescriptionInArgumentAttribute), constant.ParameterArgName, constant.Description)]
        public void ArgumentNameAndDescriptionFromParameter(string useStrategy, Type typeToTest, string name, string description)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Arguments.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(full, typeof(ParameterArgumentWithOneAliasAttribute), constant.AliasAsStringSingle)]
        [InlineData(full, typeof(ParameterArgumentWithThreeAliasesInOneAttribute), constant.AliasAsStringMultiple)]
        public void ArgumentAliasesFromParameter(string useStrategy, Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(',').Select(s => s.Trim()).ToArray();

            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Arguments.First()
                    .Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(full, typeof(ParameterArgumentWithOneAllowedValueAttribute), constant.AllowedValuesAsIntFirst)]
        [InlineData(full, typeof(ParameterArgumentWithThreeAllowedVauesInOneAttribute),
                            constant.AllowedValuesAsIntFirst, constant.AllowedValuesAsIntSecond, constant.AllowedValuesAsIntThird)]
        [InlineData(standard, typeof(ParameterArgumentWithOneAllowedValueAttribute), constant.AllowedValuesAsIntFirst)]
        [InlineData(standard, typeof(ParameterArgumentWithThreeAllowedVauesInOneAttribute),
                            constant.AllowedValuesAsIntFirst, constant.AllowedValuesAsIntSecond, constant.AllowedValuesAsIntThird)]
        public void ArgumentAllowedValuesAsFromParameter(string useStrategy, Type typeToTest, params object[] allowedValues)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Arguments.First()
                    .Should().HaveAllowedValues(allowedValues);
        }

        [Theory]
        [InlineData(full, typeof(ParameterArgumentWithIsHiddenTrueInArgumentAttribute), true)]
        [InlineData(full, typeof(ParameterArgumentWithIsHiddenFalseInArgumentAttribute), false)]
        [InlineData(full, typeof(ParameterArgumentWithIsHiddenTrue), true)]
        [InlineData(full, typeof(ParameterArgumentWithIsHiddenFalse), false)]
        [InlineData(full, typeof(ParameterArgumentWithIsHiddenTrueAsImplied), true)]
        [InlineData(standard, typeof(ParameterArgumentWithIsHiddenTrueInArgumentAttribute), true)]
        [InlineData(standard, typeof(ParameterArgumentWithIsHiddenFalseInArgumentAttribute), false)]
        public void ArgumentIsHiddenFromParameter(string useStrategy, Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Arguments.First()
                    .Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(ParameterArgumentWithRequiredTrueInArgumentAttribute), true)]
        [InlineData(full, typeof(ParameterArgumentWithRequiredFalseInArgumentAttribute), false)]
        [InlineData(full, typeof(ParameterArgumentWithRequiredTrue), true)]
        [InlineData(full, typeof(ParameterArgumentWithRequiredFalse), false)]
        [InlineData(full, typeof(ParameterArgumentWithRequiredTrueAsImplied), true)]
        [InlineData(standard, typeof(ParameterArgumentWithRequiredTrueInArgumentAttribute), true)]
        [InlineData(standard, typeof(ParameterArgumentWithRequiredFalseInArgumentAttribute), false)]
        public void ArgumentRequiredFromType(string useStrategy, Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Arguments.First()
                    .Should().HaveRequired(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(ParameterArgumentWithNoArity), false, 0, int.MaxValue)]
        [InlineData(full, typeof(ParameterArgumentWithArityLowerBoundOnly), true, 2, int.MaxValue)]
        [InlineData(full, typeof(ParameterArgumentWithArityBothBounds), true, 2, 3)]
        [InlineData(standard, typeof(ParameterArgumentWithNoArity), false, 0, int.MaxValue)]
        [InlineData(standard, typeof(ParameterArgumentWithArityLowerBoundOnly), true, 2, int.MaxValue)]
        [InlineData(standard, typeof(ParameterArgumentWithArityBothBounds), true, 2, 3)]
        public void ArgumentArityFromType(string useStrategy, Type typeToTest, bool isSet, int minCount, int maxCount)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Arguments.First()
                    .Should().HaveArity(isSet, minCount, maxCount);
        }

        [Theory]
        [InlineData(full, typeof(ParameterArgumentWithNoDefaultValue), false, null)]
        [InlineData(full, typeof(ParameterArgumentWithStringDefaultValue), true, constant.DefaultValueString)]
        [InlineData(full, typeof(ParameterArgumentWithIntegerDefaultValue), true, constant.DefaultValueInt)]
        [InlineData(standard, typeof(ParameterArgumentWithNoDefaultValue), false, null)]
        [InlineData(standard, typeof(ParameterArgumentWithStringDefaultValue), true, constant.DefaultValueString)]
        [InlineData(standard, typeof(ParameterArgumentWithIntegerDefaultValue), true, constant.DefaultValueInt)]
        public void ArgumentDefaultValuesFromType(string useStrategy, Type typeToTest, bool isSet, object value)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, method);

            descriptor.Arguments.First()
                    .Should().HaveDefaultValue(isSet, value);
        }

        #endregion

    }
}

