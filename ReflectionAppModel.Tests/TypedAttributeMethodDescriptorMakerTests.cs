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
    public class TypedAttributeMethodDescriptorMakerTests
    {
 
        private readonly Strategy strategy;


        public TypedAttributeMethodDescriptorMakerTests()
        {
            strategy = new Strategy()
                            .SetFullRules();
        }

        #region Command tests
        [Theory]
        [InlineData(typeof(MethodEmptyMethod), nameof(MethodEmptyMethod.EmptyMethod ), "")]
        [InlineData(typeof(MethodWithNameAttribute), constant.Name, "")]
        [InlineData(typeof(MethodWithNameInCommandAttribute), constant.Name, "")]
        [InlineData(typeof(MethodWithDescriptionAttribute), constant.TestMethodName, constant.Description )]
        [InlineData(typeof(MethodWithDescriptionInCommandAttribute), constant.TestMethodName, constant.Description)]
        public void NameAndDescriptionFromType(Type typeToTest, string name, string description)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Should().HaveName(name)
                    .And.HaveDescription(description);
        }


        [Theory]
        [InlineData(typeof(MethodWithOneAliasAttribute), constant.AliasAsStringSingle)]
        [InlineData(typeof(MethodWithThreeAliasesInOneAttribute), constant.AliasAsStringMultiple)]
        public void AliasesFromType(Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();

            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(typeof(MethodWithIsHiddenTrueInCommandAttribute), true)]
        [InlineData(typeof(MethodWithIsHiddenFalseInCommandAttribute), false)]
        [InlineData(typeof(MethodWithIsHiddenTrue), true)]
        [InlineData(typeof(MethodWithIsHiddenFalse), false)]
        [InlineData(typeof(MethodWithIsHiddenTrueAsImplied), true)]
        public void CommandIsHiddenFromType(Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Should().HaveIsHidden (isHidden );
        }

        [Theory]
        [InlineData(typeof(MethodWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute), true)]
        [InlineData(typeof(MethodWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute), false)]
        [InlineData(typeof(MethodWithTreatUnmatchedTokensAsErrorsTrue), true)]
        [InlineData(typeof(MethodWithTreatUnmatchedTokensAsErrorsFalse), false)]
        [InlineData(typeof(MethodWithTreatUnmatchedTokensAsErrorsTrueAsImplied), true)]
        public void CommandTreatUnmatchedTokensAsErrorsFromType(Type typeToTest, bool treatUnmatchedTokensAsErrors)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Should().HaveTreatUnmatchedTokensAsErrors(treatUnmatchedTokensAsErrors);
        }

        [Theory]
        [InlineData(typeof(MethodWithOneArgumentByArgName), constant.ArgumentName)]
        [InlineData(typeof(MethodWithTwoArgumentByArgumentName), constant.ArgumentName, constant.ArgumentName2)]
        [InlineData(typeof(MethodWithOneArgumentByAttribute), constant.ArgumentName)]
        [InlineData(typeof(MethodWithTwoArgumentsByAttribute), constant.ArgumentName, constant.ArgumentName2)]
        public void CommandWithArguments(Type typeToTest, params string[] argNames)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Should().HaveArgumentsNamed(argNames);
        }

        [Theory]
        [InlineData(typeof(MethodWithOnlyOneParameter), constant.OptionName )]
        [InlineData(typeof(MethodWithOneOptionByRemaining), constant.OptionName)]
        [InlineData(typeof(MethodWithTwoOptionsByRemaining), constant.OptionName, constant.OptionName2)]
        public void CommandWithSubOptions(Type typeToTest, params string[] argNames)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Should().HaveOptionsNamed(argNames);
        }

# endregion

        #region Option tests

        [Theory]
        [InlineData(typeof(ParameterOptionWithName), constant.Name, "")]
        [InlineData(typeof(ParameterOptionWithNameAttribute), constant.Name, "")]
        [InlineData(typeof(ParameterOptionWithNameInOptionAttribute), constant.Name, "")]
        [InlineData(typeof(ParameterOptionWithDescriptionAttribute), constant.ParameterOptionName, constant.Description)]
        [InlineData(typeof(ParameterOptionWithDescriptionInOptionAttribute), constant.ParameterOptionName, constant.Description)]
        public void OptionNameAndDescriptionFromParameter(Type typeToTest, string name, string description)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Options.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(typeof(ParameterOptionWithOneAliasAttribute), constant.AliasAsStringSingle)]
        [InlineData(typeof(ParameterOptionWithThreeAliasesInOneAttribute), constant.AliasAsStringMultiple)]
        public void OptionAliasesFromParameter(Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();

            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Options.First()
                    .Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(typeof(ParameterOptionWithIsHiddenTrueInOptionAttribute), true)]
        [InlineData(typeof(ParameterOptionWithIsHiddenFalseInOptionAttribute), false)]
        [InlineData(typeof(ParameterOptionWithIsHiddenTrue), true)]
        [InlineData(typeof(ParameterOptionWithIsHiddenFalse), false)]
        [InlineData(typeof(ParameterOptionWithIsHiddenTrueAsImplied), true)]
        public void OptionIsHiddenFromParameter(Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Options.First()
                    .Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(typeof(ParameterOptionWithRequiredTrueInOptionAttribute), true)]
        [InlineData(typeof(ParameterOptionWithRequiredFalseInOptionAttribute), false)]
        [InlineData(typeof(ParameterOptionWithRequiredTrue), true)]
        [InlineData(typeof(ParameterOptionWithRequiredFalse), false)]
        [InlineData(typeof(ParameterOptionWithRequiredTrueAsImplied), true)]
        public void OptionRequiredFromParameter(Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Options.First()
                    .Should().HaveRequired(isHidden);
        }

        [Theory]
        [InlineData(typeof(ParameterOptionArgumentWithNoDefaultValue), false, null)]
        [InlineData(typeof(ParameterOptionArgumentWithStringDefaultValue), true, constant.DefaultValueString)]
        [InlineData(typeof(ParameterOptionArgumentWithIntegerDefaultValue), true, constant.DefaultValueInt)]
        public void OptionDefaultValueFromParameter(Type typeToTest, bool isSet, object value)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Options.First().Arguments.First()
                    .Should().HaveDefaultValue(isSet, value);
        }

        [Theory]
        [InlineData(typeof(ParameterOptionWithName), typeof(string))]
        [InlineData(typeof(ParameterOptionArgumentForIntegerType), typeof(int))]
        public void OptionWithArguments(Type typeToTest, Type argType)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Options.First().Arguments.First()
                    .Should().HaveArgumentType(argType);
        }

        #endregion

        #region Argument Tests

        [Theory]
        [InlineData(typeof(ParameterArgumentWithName), constant.Name, "")]
        [InlineData(typeof(ParameterArgumentWithNameAttribute), constant.Name, "")]
        [InlineData(typeof(ParameterArgumentWithNameInArgumentAttribute), constant.Name, "")]
        [InlineData(typeof(ParameterArgumentWithDescriptionAttribute), constant.ParameterArgName, constant.Description)]
        [InlineData(typeof(ParameterArgumentWithDescriptionInArgumentAttribute), constant.ParameterArgName, constant.Description)]
        public void ArgumentNameAndDescriptionFromParameter(Type typeToTest, string name, string description)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Arguments.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(typeof(ParameterArgumentWithOneAliasAttribute), constant.AliasAsStringSingle)]
        [InlineData(typeof(ParameterArgumentWithThreeAliasesInOneAttribute), constant.AliasAsStringMultiple)]
        public void ArgumentAliasesFromParameter(Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();

            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Arguments.First()
                    .Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(typeof(ParameterArgumentWithOneAllowedValueAttribute), constant.AllowedValuesAsIntFirst)]
        [InlineData(typeof(ParameterArgumentWithThreeAllowedVauesInOneAttribute),
                            constant.AllowedValuesAsIntFirst, constant.AllowedValuesAsIntSecond, constant.AllowedValuesAsIntThird)]
        public void ArgumentAllowedValuesAsFromParameter(Type typeToTest, params object[] allowedValues)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Arguments.First()
                    .Should().HaveAllowedValues(allowedValues);
        }

        [Theory]
        [InlineData(typeof(ParameterArgumentWithIsHiddenTrueInArgumentAttribute), true)]
        [InlineData(typeof(ParameterArgumentWithIsHiddenFalseInArgumentAttribute), false)]
        [InlineData(typeof(ParameterArgumentWithIsHiddenTrue), true)]
        [InlineData(typeof(ParameterArgumentWithIsHiddenFalse), false)]
        [InlineData(typeof(ParameterArgumentWithIsHiddenTrueAsImplied), true)]
        public void ArgumentIsHiddenFromParameter(Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Arguments.First()
                    .Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(typeof(ParameterArgumentWithRequiredTrueInArgumentAttribute), true)]
        [InlineData(typeof(ParameterArgumentWithRequiredFalseInArgumentAttribute), false)]
        [InlineData(typeof(ParameterArgumentWithRequiredTrue), true)]
        [InlineData(typeof(ParameterArgumentWithRequiredFalse), false)]
        [InlineData(typeof(ParameterArgumentWithRequiredTrueAsImplied), true)]
        public void ArgumentRequiredFromType(Type typeToTest, bool isHidden)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Arguments.First()
                    .Should().HaveRequired(isHidden);
        }

        [Theory]
        [InlineData(typeof(ParameterArgumentWithNoArity), false, 0, int.MaxValue)]
        [InlineData(typeof(ParameterArgumentWithArityLowerBoundOnly), true, 2, int.MaxValue)]
        [InlineData(typeof(ParameterArgumentWithArityBothBounds), true, 2, 3)]
        public void ArgumentArityFromType(Type typeToTest, bool isSet, int minCount, int maxCount)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Arguments.First()
                    .Should().HaveArity(isSet, minCount, maxCount);
        }

        [Theory]
        [InlineData(typeof(ParameterArgumentWithNoDefaultValue), false, null)]
        [InlineData(typeof(ParameterArgumentWithStringDefaultValue), true, constant.DefaultValueString)]
        [InlineData(typeof(ParameterArgumentWithIntegerDefaultValue), true, constant.DefaultValueInt)]
        public void ArgumentDefaultValuesFromType(Type typeToTest, bool isSet, object value)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Arguments.First()
                    .Should().HaveDefaultValue(isSet, value);
        }

        #endregion

    }
}

