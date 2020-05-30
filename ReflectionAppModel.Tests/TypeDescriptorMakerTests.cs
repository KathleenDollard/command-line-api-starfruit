using FluentAssertions;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests.Maker;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.Linq;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class TypeDescriptorMakerTests
    {
        internal const string Name = "George";
        internal const string NameForEmpty = "DummyName";
        internal const string Description = "Awesome description!";
        internal const string AliasAsStringMuitple = "a,b,c";
        internal const string AliasAsStringSingle = "x";
        internal const string ArgumentName = "Red";
        internal const string ArgumentName2 = "Blue";
        internal const string OptionName = "East";
        internal const string OptionName2 = "West";
        internal const string PropertyOptionName = "Prop";
        internal const string PropertyArgName = "Prop";
        internal const string DefaultValueString = "MyDefault";
        internal const int DefaultValueInt = 42;
  
        private readonly Strategy strategy;


        public TypeDescriptorMakerTests()
        {
            strategy = new Strategy()
                            .SetReflectionRules();
        }

        #region CommandTests
        [Theory]
        [InlineData(typeof(EmptyType), nameof(EmptyType), "")]
        [InlineData(typeof(TypeWithNameAttribute), Name, "")]
        [InlineData(typeof(TypeWithNameInCommandAttribute), Name, "")]
        [InlineData(typeof(TypeWithDescriptionAttribute), nameof(TypeWithDescriptionAttribute), Description)]
        [InlineData(typeof(TypeWithDescriptionInCommandAttribute), nameof(TypeWithDescriptionInCommandAttribute), Description)]
        public void CommandNameAndDescriptionFromType(Type typeToTest, string name, string description)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(typeof(TypeWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(typeof(TypeWithThreeAliasesInOneAttribute), AliasAsStringMuitple)]
        public void CommandAliasesFromType(Type typeToTest, string aliasesAsString)
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

        [Theory]
        [InlineData(typeof(TypeWithOneArgumentByArgName), ArgumentName)]
        [InlineData(typeof(TypeWithTwoArgumentByArgumentName), ArgumentName, ArgumentName2)]
        [InlineData(typeof(TypeWithOneArgumentByAttribute), ArgumentName)]
        [InlineData(typeof(TypeWithTwoArgumentsByAttribute), ArgumentName, ArgumentName2)]
        public void CommandWithArguments(Type typeToTest, params string[] argNames)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveArgumentsNamed(argNames);
        }

        [Theory]
        [InlineData(typeof(TypeWithOneCommandByDerivedType), "A")]
        [InlineData(typeof(TypeWithTwoCommandsByDerivedType), "A", "B")]
        public void CommandWithSubCommands(Type typeToTest, params string[] argNames)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveSubCommandsNamed(argNames);
        }

        [Theory]
        [InlineData(typeof(TypeWithOnlyOneProperty), OptionName )]
        [InlineData(typeof(TypeWithOneOptionByRemaining), OptionName)]
        [InlineData(typeof(TypeWithTwoOptionsByRemaining), OptionName, OptionName2)]
        public void CommandWithSubOptions(Type typeToTest, params string[] argNames)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Should().HaveOptionsNamed(argNames);
        }
        #endregion


        #region Option tests

        [Theory]
        [InlineData(typeof(PropertyOptionWithName), Name, "")]
        [InlineData(typeof(PropertyOptionWithNameAttribute), Name, "")]
        [InlineData(typeof(PropertyOptionWithNameInOptionAttribute), Name, "")]
        [InlineData(typeof(PropertyOptionWithDescriptionAttribute), PropertyOptionName, Description)]
        [InlineData(typeof(PropertyOptionWithDescriptionInOptionAttribute), PropertyOptionName, Description)]
        public void OptionNameAndDescriptionFromProperty(Type typeToTest, string name, string description)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Options.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(typeof(PropertyOptionWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(typeof(PropertyOptionWithThreeAliasesInOneAttribute), AliasAsStringMuitple)]
        public void OptionAliasesFromProperty(Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();

            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Options.First()
                    .Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(typeof(PropertyOptionWithIsHiddenTrueInOptionAttribute), true)]
        [InlineData(typeof(PropertyOptionWithIsHiddenFalseInOptionAttribute), false)]
        [InlineData(typeof(PropertyOptionWithIsHiddenTrue), true)]
        [InlineData(typeof(PropertyOptionWithIsHiddenFalse), false)]
        [InlineData(typeof(PropertyOptionWithIsHiddenTrueAsImplied), true)]
        public void OptionIsHiddenFromProperty(Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Options.First()
                    .Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(typeof(PropertyOptionWithRequiredTrueInOptionAttribute), true)]
        [InlineData(typeof(PropertyOptionWithRequiredFalseInOptionAttribute), false)]
        [InlineData(typeof(PropertyOptionWithRequiredTrue), true)]
        [InlineData(typeof(PropertyOptionWithRequiredFalse), false)]
        [InlineData(typeof(PropertyOptionWithRequiredTrueAsImplied), true)]
        public void OptionRequiredFromProperty(Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Options.First()
                    .Should().HaveRequired(isHidden);
        }

        [Theory]
        [InlineData(typeof(PropertyOptionArgumentWithNoDefaultValue),false, null )]
        [InlineData(typeof(PropertyOptionArgumentWithStringDefaultValue), true, DefaultValueString )]
        [InlineData(typeof(PropertyOptionArgumentWithIntegerDefaultValue), true, DefaultValueInt)]
        public void OptionDefaultValueFromProperty(Type typeToTest, bool isSet, object value)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Options.First().Arguments.First()
                    .Should().HaveDefaultValue(isSet, value);
        }

        [Theory]
        [InlineData(typeof(PropertyOptionWithName), typeof(string))]
        [InlineData(typeof(PropertyOptionArgumentForIntegerType), typeof(int))]
        public void OptionWithArguments(Type typeToTest,Type argType)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Options.First().Arguments.First()
                    .Should().HaveArgumentType(argType);
        }

        #endregion

        #region Argument Tests

        [Theory]
        [InlineData(typeof(PropertyArgumentWithName), Name, "")]
        [InlineData(typeof(PropertyArgumentWithNameAttribute), Name, "")]
        [InlineData(typeof(PropertyArgumentWithNameInArgumentAttribute), Name, "")]
        [InlineData(typeof(PropertyArgumentWithDescriptionAttribute), PropertyArgName, Description)]
        [InlineData(typeof(PropertyArgumentWithDescriptionInArgumentAttribute), PropertyArgName, Description)]
        public void ArgumentNameAndDescriptionFromProperty(Type typeToTest, string name, string description)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(typeof(PropertyArgumentWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(typeof(PropertyArgumentWithThreeAliasesInOneAttribute), AliasAsStringMuitple)]
        public void ArgumentAliasesFromProperty(Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();

            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(typeof(PropertyArgumentWithIsHiddenTrueInArgumentAttribute), true)]
        [InlineData(typeof(PropertyArgumentWithIsHiddenFalseInArgumentAttribute), false)]
        [InlineData(typeof(PropertyArgumentWithIsHiddenTrue), true)]
        [InlineData(typeof(PropertyArgumentWithIsHiddenFalse), false)]
        [InlineData(typeof(PropertyArgumentWithIsHiddenTrueAsImplied), true)]
        public void ArgumentIsHiddenFromProperty(Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(typeof(PropertyArgumentWithRequiredTrueInArgumentAttribute), true)]
        [InlineData(typeof(PropertyArgumentWithRequiredFalseInArgumentAttribute), false)]
        [InlineData(typeof(PropertyArgumentWithRequiredTrue), true)]
        [InlineData(typeof(PropertyArgumentWithRequiredFalse), false)]
        [InlineData(typeof(PropertyArgumentWithRequiredTrueAsImplied), true)]
        public void ArgumentRequiredFromType(Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveRequired(isHidden);
        }

        [Theory]
        [InlineData(typeof(PropertyArgumentWithNoArity), false, 0, int.MaxValue)]
        [InlineData(typeof(PropertyArgumentWithArityLowerBoundOnly), true,2, int.MaxValue )]
        [InlineData(typeof(PropertyArgumentWithArityUpperBoundOnly), true,0,3)]
        [InlineData(typeof(PropertyArgumentWithArityBothBounds), true, 2,3)]
        public void ArgumentArityFromType(Type typeToTest, bool isSet, int minCount, int maxCount)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveArity(isSet, minCount, maxCount  );
        }

        [Theory]
        [InlineData(typeof(PropertyArgumentWithNoDefaultValue), false, null)]
        [InlineData(typeof(PropertyArgumentWithStringDefaultValue), true, DefaultValueString)]
        [InlineData(typeof(PropertyArgumentWithIntegerDefaultValue), true, DefaultValueInt)]
        public void ArgumentDefaultValuesFromType(Type typeToTest, bool isSet, object value)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveDefaultValue(isSet, value);
        }

        #endregion

    }
}

