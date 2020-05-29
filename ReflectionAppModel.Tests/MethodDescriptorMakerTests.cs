using FluentAssertions;
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
        public const string Name = "George";
        public const string NameForEmpty = "DummyName";
        public const string Description = "Awesome description!";
        public const string AliasAsStringMuitple = "a,b,c";
        public const string AliasAsStringSingle = "x";
        public const string ArgumentName = "Red";
        public const string ArgumentName2 = "Blue";
        public const string OptionName = "East";
        public const string OptionName2 = "West";
        private readonly Strategy strategy;


        public MethodDescriptorMakerTests()
        {
            strategy = new Strategy()
                            .SetReflectionRules();
        }


        [Theory]
        [InlineData(typeof(TypeWithEmptyMethod), nameof(TypeWithEmptyMethod), default)]
        [InlineData(typeof(TypeWithMethodWithNameAttribute), Name, default)]
        [InlineData(typeof(TypeWithMethodWithDescriptionAttribute), Name, default)]
        [InlineData(typeof(TypeWithMethodWithNameInCommandAttribute), nameof(TypeWithMethodWithNameInCommandAttribute), Description)]
        [InlineData(typeof(TypeWithMethodWithDescriptionInCommandAttribute), nameof(TypeWithMethodWithDescriptionInCommandAttribute), Description)]
        public void NameAndDescriptionFromType(Type typeToTest, string name, string description)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveName(name)
                    .And.HaveDescription(description);
        }


        [Theory]
        [InlineData(typeof(TypeWithMethodWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(typeof(TypeWithMethodWithThreeAliasesInOneAttribute), AliasAsStringMuitple)]
        public void AliasesFromType(Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();

            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(typeof(TypeWithMethodWithIsHiddenTrueInCommandAttribute), true)]
        [InlineData(typeof(TypeWithMethodWithIsHiddenFalseInCommandAttribute), false)]
        [InlineData(typeof(TypeWithMethodWithIsHiddenTrue), true)]
        [InlineData(typeof(TypeWithMethodWithIsHiddenFalse), false)]
        [InlineData(typeof(TypeWithMethodWithIsHiddenTrueAsImplied), true)]
        public void CommandIsHiddenFromType(Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveIsHidden (isHidden );
        }

        [Theory]
        [InlineData(typeof(TypeWithMethodWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute), true)]
        [InlineData(typeof(TypeWithMethodWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute), false)]
        [InlineData(typeof(TypeWithMethodWithTreatUnmatchedTokensAsErrorsTrue), true)]
        [InlineData(typeof(TypeWithMethodWithTreatUnmatchedTokensAsErrorsFalse), false)]
        [InlineData(typeof(TypeWithMethodWithTreatUnmatchedTokensAsErrorsTrueAsImplied), true)]
        public void CommandTreatUnmatchedTokensAsErrorsFromType(Type typeToTest, bool treatUnmatchedTokensAsErrors)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveTreatUnmatchedTokensAsErrors(treatUnmatchedTokensAsErrors);
        }

        [Theory]
        [InlineData(typeof(TypeWithMethodWithOneArgumentByArgName), ArgumentName)]
        [InlineData(typeof(TypeWithMethodWithTwoArgumentByArgumentName), ArgumentName, ArgumentName2)]
        [InlineData(typeof(TypeWithMethodWithOneArgumentByAttribute), ArgumentName)]
        [InlineData(typeof(TypeWithMethodWithTwoArgumentsByAttribute), ArgumentName, ArgumentName2)]
        public void CommandWithArguments(Type typeToTest, params string[] argNames)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveArgumentsNamed(argNames);
        }

        [Theory]
        [InlineData(typeof(TypeWithMethodWithOnlyOneParameter), OptionName )]
        [InlineData(typeof(TypeWithMethodWithOneOptionByRemaining), OptionName)]
        [InlineData(typeof(TypeWithMethodWithTwoOptionsByRemaining), OptionName, OptionName2)]
        public void CommandWithSubOptions(Type typeToTest, params string[] argNames)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveOptionsNamed(argNames);
        }

    }
}

