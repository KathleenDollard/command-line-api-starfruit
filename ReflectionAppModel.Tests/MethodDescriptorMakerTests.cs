using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests.Maker;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.Linq;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class MethodDescriptorMakerTests
    {
        internal const string Name = "George";
        internal const string NameForEmpty = "DummyName";
        internal const string TestMethodName = "Method";
        internal const string Description = "Awesome description!";
        internal const string AliasAsStringMuitple = "a,b,c";
        internal const string AliasAsStringSingle = "x";
        internal const string ArgumentName = "Red";
        internal const string ArgumentName2 = "Blue";
        internal const string OptionName = "East";
        internal const string OptionName2 = "West";
        internal const string ParameterOptionName = "Param";
        internal const string ParameterArgName = "Param";
        internal const string DefaultValueString = "MyDefault";
        internal const int DefaultValueInt = 42;

        private readonly Strategy strategy;


        public MethodDescriptorMakerTests()
        {
            strategy = new Strategy()
                            .SetReflectionRules();
        }

        #region Command tests
        [Theory]
        [InlineData(typeof(MethodEmptyMethod), nameof(MethodEmptyMethod.EmptyMethod ), "")]
        [InlineData(typeof(MethodWithNameAttribute), Name, "")]
        [InlineData(typeof(MethodWithNameInCommandAttribute), Name, "")]
        [InlineData(typeof(MethodWithDescriptionAttribute), TestMethodName, Description )]
        [InlineData(typeof(MethodWithDescriptionInCommandAttribute), TestMethodName, Description)]
        public void NameAndDescriptionFromType(Type typeToTest, string name, string description)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Should().HaveName(name)
                    .And.HaveDescription(description);
        }


        [Theory]
        [InlineData(typeof(MethodWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(typeof(MethodWithThreeAliasesInOneAttribute), AliasAsStringMuitple)]
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
        [InlineData(typeof(MethodWithOneArgumentByArgName), ArgumentName)]
        [InlineData(typeof(MethodWithTwoArgumentByArgumentName), ArgumentName, ArgumentName2)]
        [InlineData(typeof(MethodWithOneArgumentByAttribute), ArgumentName)]
        [InlineData(typeof(MethodWithTwoArgumentsByAttribute), ArgumentName, ArgumentName2)]
        public void CommandWithArguments(Type typeToTest, params string[] argNames)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Should().HaveArgumentsNamed(argNames);
        }

        [Theory]
        [InlineData(typeof(MethodWithOnlyOneParameter), OptionName )]
        [InlineData(typeof(MethodWithOneOptionByRemaining), OptionName)]
        [InlineData(typeof(MethodWithTwoOptionsByRemaining), OptionName, OptionName2)]
        public void CommandWithSubOptions(Type typeToTest, params string[] argNames)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Should().HaveOptionsNamed(argNames);
        }

# endregion

        #region Option tests

        [Theory]
        [InlineData(typeof(ParameterOptionWithName), Name, "")]
        [InlineData(typeof(ParameterOptionWithNameAttribute), Name, "")]
        [InlineData(typeof(ParameterOptionWithNameInOptionAttribute), Name, "")]
        [InlineData(typeof(ParameterOptionWithDescriptionAttribute), ParameterOptionName, Description)]
        [InlineData(typeof(ParameterOptionWithDescriptionInOptionAttribute), ParameterOptionName, Description)]
        public void OptionNameAndDescriptionFromParameter(Type typeToTest, string name, string description)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Options.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(typeof(ParameterOptionWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(typeof(ParameterOptionWithThreeAliasesInOneAttribute), AliasAsStringMuitple)]
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
        [InlineData(typeof(ParameterOptionArgumentWithStringDefaultValue), true, DefaultValueString)]
        [InlineData(typeof(ParameterOptionArgumentWithIntegerDefaultValue), true, DefaultValueInt)]
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
        [InlineData(typeof(ParameterArgumentWithName), Name, "")]
        [InlineData(typeof(ParameterArgumentWithNameAttribute), Name, "")]
        [InlineData(typeof(ParameterArgumentWithNameInArgumentAttribute), Name, "")]
        [InlineData(typeof(ParameterArgumentWithDescriptionAttribute), ParameterArgName, Description)]
        [InlineData(typeof(ParameterArgumentWithDescriptionInArgumentAttribute), ParameterArgName, Description)]
        public void ArgumentNameAndDescriptionFromParameter(Type typeToTest, string name, string description)
        {
            var method = typeToTest.GetMethods().First();
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);

            descriptor.Arguments.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(typeof(ParameterArgumentWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(typeof(ParameterArgumentWithThreeAliasesInOneAttribute), AliasAsStringMuitple)]
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
        [InlineData(typeof(ParameterArgumentWithStringDefaultValue), true, DefaultValueString)]
        [InlineData(typeof(ParameterArgumentWithIntegerDefaultValue), true, DefaultValueInt)]
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

