using FluentAssertions;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests.Maker;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.Linq;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class NewDescriptorMakerTests
    {
        public const string Name = "George";
        public const string NameForEmpty = "DummyName";
        public const string Description = "Awesome description!";
        public const string AliasAsStringMuitple = "a,b,c";
        public const string AliasAsStringSingle = "x";
        private readonly Strategy strategy;


        public NewDescriptorMakerTests()
        {
            strategy = new Strategy()
                            .SetReflectionRules();
        }


        [Theory]
        [InlineData(typeof(EmptyType), nameof(EmptyType), default)]
        [InlineData(typeof(TypeWithNameAttribute), Name, default)]
        [InlineData(typeof(TypeWithNameInCommandAttribute), Name, default)]
        [InlineData(typeof(TypeWithDescriptionAttribute), nameof(TypeWithDescriptionAttribute), Description)]
        [InlineData(typeof(TypeWithDescriptionInCommandAttribute), nameof(TypeWithDescriptionInCommandAttribute), Description)]
        public void NameAndDescriptionFromType(Type typeToTest, string name, string description)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveName(name)
                    .And.HaveDescription(description);
        }


        [Theory]
        [InlineData(typeof(TypeWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(typeof(TypeWithThreeAliasesInOneAttribute), AliasAsStringMuitple)]
        public void AliasesFromType(Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();

            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(typeof(TypeWithIsHiddenTrueInCommandAttribute), true)]
        [InlineData(typeof(TypeWithIsHiddenFalseInCommandAttribute), false)]
        [InlineData(typeof(TypeWithIsHiddenTrue), true)]
        [InlineData(typeof(TypeWithIsHiddenFalse), false)]
        [InlineData(typeof(TypeWithIsHiddenTrueAsImplied), true)]
        public void CommandIsHiddenFromType(Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveIsHidden (isHidden );
        }

        [Theory]
        [InlineData(typeof(TypeWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute), true)]
        [InlineData(typeof(TypeWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute), false)]
        [InlineData(typeof(TypeWithTreatUnmatchedTokensAsErrorsTrue), true)]
        [InlineData(typeof(TypeWithTreatUnmatchedTokensAsErrorsFalse), false)]
        [InlineData(typeof(TypeWithTreatUnmatchedTokensAsErrorsTrueAsImplied), true)]
        public void CommandTreatUnmatchedTokensAsErrorsFromType(Type typeToTest, bool treatUnmatchedTokensAsErrors)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveTreatUnmatchedTokensAsErrors(treatUnmatchedTokensAsErrors);
        }

    }
}

