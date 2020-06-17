using FluentAssertions;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests.Maker;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests.TypedAttributes;
using System.Linq;
using System.Reflection;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class TypeDescriptorMakerTests
    {
        internal const string Name = "George";
        internal const string NameForEmpty = "DummyName";
        internal const string Description = "Awesome description!";
        internal const string AliasAsStringMultiple = "a,b,c";
        internal const string AliasAsStringSingle = "x";
        internal const string OptionAliasAsStringMultiple = "-a,-b,-c";
        internal const string OptionAliasAsStringSingle = "-x";
        internal const int AllowedValuesAsIntFirst = 3;
        internal const int AllowedValuesAsIntSecond = 5;
        internal const int AllowedValuesAsIntThird = 7;
        internal const string ArgumentName = "Red";
        internal const string ArgumentName2 = "Blue";
        internal const string OptionName = "East";
        internal const string OptionName2 = "West";
        internal const string PropertyOptionName = "Prop";
        internal const string PropertyArgName = "Prop";
        internal const string DefaultValueString = "MyDefault";
        internal const int DefaultValueInt = 42;

        internal const string TestMethodName = "Method";
        internal const string ParameterOptionName = "Param";
        internal const string ParameterArgName = "Param";

        private readonly Strategy fullStrategy;
        private readonly Strategy standardStrategy;
        private const string full = "Full";
        private const string standard = "Standard";

        public TypeDescriptorMakerTests()
        {
            fullStrategy = new Strategy().SetFullRules();
            standardStrategy = new Strategy().SetStandardRules();
        }

        #region CommandTests
        [Theory]
        [InlineData(full, typeof(EmptyType), nameof(EmptyType), "")]
        [InlineData(full, typeof(TypeWithNameAttribute), Name, "")]
        [InlineData(full, typeof(TypeWithNameInCommandAttribute), Name, "")]
        [InlineData(full, typeof(TypeWithDescriptionAttribute), nameof(TypeWithDescriptionAttribute), Description)]
        [InlineData(full, typeof(TypeWithDescriptionInCommandAttribute), nameof(TypeWithDescriptionInCommandAttribute), Description)]
        [InlineData(standard, typeof(EmptyType), nameof(EmptyType), "")]
        [InlineData(standard, typeof(TypeWithNameInCommandAttribute), Name, "")]
        [InlineData(standard, typeof(TypeWithDescriptionInCommandAttribute), nameof(TypeWithDescriptionInCommandAttribute), Description)]
        public void CommandNameAndDescriptionFromType(string useStrategy, Type typeToTest, string name, string description)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(full, typeof(TypeWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(full, typeof(TypeWithThreeAliasesInOneAttribute), AliasAsStringMultiple)]
        public void CommandAliasesFromType(string useStrategy, Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(',').Select(s => s.Trim()).ToArray();

            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(full, typeof(TypeWithIsHiddenTrueInCommandAttribute), true)]
        [InlineData(full, typeof(TypeWithIsHiddenFalseInCommandAttribute), false)]
        [InlineData(full, typeof(TypeWithIsHiddenTrue), true)]
        [InlineData(full, typeof(TypeWithIsHiddenFalse), false)]
        [InlineData(full, typeof(TypeWithIsHiddenTrueAsImplied), true)]
        [InlineData(standard, typeof(TypeWithIsHiddenTrueInCommandAttribute), true)]
        [InlineData(standard, typeof(TypeWithIsHiddenFalseInCommandAttribute), false)]
        public void CommandIsHiddenFromType(string useStrategy, Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(TypeWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute), true)]
        [InlineData(full, typeof(TypeWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute), false)]
        [InlineData(full, typeof(TypeWithTreatUnmatchedTokensAsErrorsTrue), true)]
        [InlineData(full, typeof(TypeWithTreatUnmatchedTokensAsErrorsFalse), false)]
        [InlineData(full, typeof(TypeWithTreatUnmatchedTokensAsErrorsTrueAsImplied), true)]
        [InlineData(standard, typeof(TypeWithTreatUnmatchedTokensAsErrorsTrueInCommandAttribute), true)]
        [InlineData(standard, typeof(TypeWithTreatUnmatchedTokensAsErrorsFalseInCommandAttribute), false)]
        public void CommandTreatUnmatchedTokensAsErrorsFromType(string useStrategy, Type typeToTest, bool treatUnmatchedTokensAsErrors)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Should().HaveTreatUnmatchedTokensAsErrors(treatUnmatchedTokensAsErrors);
        }

        [Theory]
        [InlineData(full, typeof(TypeWithOneArgumentByArgName), ArgumentName)]
        [InlineData(full, typeof(TypeWithTwoArgumentByArgumentName), ArgumentName, ArgumentName2)]
        [InlineData(full, typeof(TypeWithOneArgumentByAttribute), ArgumentName)]
        [InlineData(full, typeof(TypeWithTwoArgumentsByAttribute), ArgumentName, ArgumentName2)]
        [InlineData(standard, typeof(TypeWithOneArgumentByArgName), ArgumentName)]
        [InlineData(standard, typeof(TypeWithOneArgumentByAttribute), ArgumentName)]
        [InlineData(standard, typeof(TypeWithTwoArgumentsByAttribute), ArgumentName, ArgumentName2)]
        public void CommandWithArguments(string useStrategy, Type typeToTest, params string[] argNames)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Should().HaveArgumentsNamed(argNames);
        }

        [Theory]
        [InlineData(full, typeof(TypeWithOneCommandByDerivedType), "A")]
        [InlineData(full, typeof(TypeWithTwoCommandsByDerivedType), "A", "B")]
        [InlineData(standard, typeof(TypeWithOneCommandByDerivedType), "A")]
        [InlineData(standard, typeof(TypeWithTwoCommandsByDerivedType), "A", "B")]
        public void CommandWithSubCommands(string useStrategy, Type typeToTest, params string[] argNames)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Should().HaveSubCommandsNamed(argNames);
        }

        [Theory]
        [InlineData(full, typeof(TypeWithOnlyOneProperty), "--" + OptionName)]
        [InlineData(full, typeof(TypeWithOneOptionByRemaining), "--" + OptionName)]
        [InlineData(full, typeof(TypeWithTwoOptionsByRemaining), "--" + OptionName, "--" + OptionName2)]
        [InlineData(standard, typeof(TypeWithOnlyOneProperty), "--" + OptionName)]
        [InlineData(standard, typeof(TypeWithOneOptionByRemaining), "--" + OptionName)]
        [InlineData(standard, typeof(TypeWithTwoOptionsByRemaining), "--" + OptionName, "--" + OptionName2)]
        public void CommandWithSubOptions(string useStrategy, Type typeToTest, params string[] argNames)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Should().HaveOptionsNamed(argNames);
        }

        [Theory]
        [InlineData(full, typeof(TypeWithInvokeMethod), "Invoke", "")]
        [InlineData(full, typeof(TypeWithTwoInvokeMethods), "Invoke", "p1,p2", typeof(string), typeof(int))]
        public void CommandWithInvokeMethods(string useStrategy, Type typeToTest, string name, string parameterNames, params Type[] parameterTypes)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);
            descriptor.Should().NotBeNull();
            descriptor.InvokeMethod.Should().NotBeNull();
            if (descriptor.InvokeMethod is null)
            { return; }
            descriptor.Should().HaveInvokeMethodInfo(name, parameterTypes.Count());
            var paramStuff = parameterNames.Split(',').Select(x => x.Trim()).Zip(parameterTypes, (name, type) => (name, type)).ToArray();
            for (int i = 0; i < paramStuff.Count(); i++)
            {
                var actual = descriptor.InvokeMethod.ChildCandidates.Skip(i).First().Item as ParameterInfo;
                actual.Should().NotBeNull();
                if (actual is null)
                { continue; }
                actual.Name.Should().Be(paramStuff[i].name);
                actual.ParameterType.Should().Be(paramStuff[i].type);
            }
        }
        #endregion

        #region Option tests

        [Theory]
        [InlineData(full, typeof(PropertyOptionWithName), "--" + Name, "")]
        [InlineData(full, typeof(PropertyOptionWithNameAttribute), "--" + Name, "")]
        [InlineData(full, typeof(PropertyOptionWithNameInOptionAttribute), "--" + Name, "")]
        [InlineData(full, typeof(PropertyOptionWithDescriptionAttribute), "--" + PropertyOptionName, Description)]
        [InlineData(full, typeof(PropertyOptionWithDescriptionInOptionAttribute), "--" + PropertyOptionName, Description)]
        [InlineData(standard, typeof(PropertyOptionWithName), "--" + Name, "")]
        [InlineData(standard, typeof(PropertyOptionWithNameInOptionAttribute), "--" + Name, "")]
        [InlineData(standard, typeof(PropertyOptionWithDescriptionInOptionAttribute), "--" + PropertyOptionName, Description)]
        public void OptionNameAndDescriptionFromProperty(string useStrategy, Type typeToTest, string name, string description)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Options.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(full, typeof(PropertyOptionWithOneAliasAttribute), OptionAliasAsStringSingle)]
        [InlineData(full, typeof(PropertyOptionWithThreeAliasesInOneAttribute), OptionAliasAsStringMultiple)]
        public void OptionAliasesFromProperty(string useStrategy, Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(',').Select(s => s.Trim()).ToArray();

            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Options.First()
                    .Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(full, typeof(PropertyOptionWithIsHiddenTrueInOptionAttribute), true)]
        [InlineData(full, typeof(PropertyOptionWithIsHiddenFalseInOptionAttribute), false)]
        [InlineData(full, typeof(PropertyOptionWithIsHiddenTrue), true)]
        [InlineData(full, typeof(PropertyOptionWithIsHiddenFalse), false)]
        [InlineData(full, typeof(PropertyOptionWithIsHiddenTrueAsImplied), true)]
        [InlineData(standard, typeof(PropertyOptionWithIsHiddenTrueInOptionAttribute), true)]
        [InlineData(standard, typeof(PropertyOptionWithIsHiddenFalseInOptionAttribute), false)]
        public void OptionIsHiddenFromProperty(string useStrategy, Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Options.First()
                    .Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(PropertyOptionWithRequiredTrueInOptionAttribute), true)]
        [InlineData(full, typeof(PropertyOptionWithRequiredFalseInOptionAttribute), false)]
        [InlineData(full, typeof(PropertyOptionWithRequiredTrue), true)]
        [InlineData(full, typeof(PropertyOptionWithRequiredFalse), false)]
        [InlineData(full, typeof(PropertyOptionWithRequiredTrueAsImplied), true)]
        [InlineData(standard, typeof(PropertyOptionWithRequiredTrueInOptionAttribute), true)]
        [InlineData(standard, typeof(PropertyOptionWithRequiredFalseInOptionAttribute), false)]
        public void OptionRequiredFromProperty(string useStrategy, Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Options.First()
                    .Should().HaveRequired(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(PropertyOptionArgumentWithNoDefaultValue), false, null)]
        [InlineData(full, typeof(PropertyOptionArgumentWithStringDefaultValue), true, DefaultValueString)]
        [InlineData(full, typeof(PropertyOptionArgumentWithIntegerDefaultValue), true, DefaultValueInt)]
        [InlineData(standard, typeof(PropertyOptionArgumentWithNoDefaultValue), false, null)]
        [InlineData(standard, typeof(PropertyOptionArgumentWithStringDefaultValue), true, DefaultValueString)]
        [InlineData(standard, typeof(PropertyOptionArgumentWithIntegerDefaultValue), true, DefaultValueInt)]
        public void OptionDefaultValueFromProperty(string useStrategy, Type typeToTest, bool isSet, object value)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Options.First().Arguments.First()
                    .Should().HaveDefaultValue(isSet, value);
        }

        [Theory]
        [InlineData(full, typeof(PropertyOptionWithName), typeof(string))]
        [InlineData(full, typeof(PropertyOptionArgumentForIntegerType), typeof(int))]
        [InlineData(standard, typeof(PropertyOptionWithName), typeof(string))]
        [InlineData(standard, typeof(PropertyOptionArgumentForIntegerType), typeof(int))]
        public void OptionWithArguments(string useStrategy, Type typeToTest, Type argType)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Options.First().Arguments.First()
                    .Should().HaveArgumentType(argType);
        }

        #endregion

        #region Argument Tests

        [Theory]
        [InlineData(full, typeof(PropertyArgumentWithName), Name, "")]
        [InlineData(full, typeof(PropertyArgumentWithNameAttribute), Name, "")]
        [InlineData(full, typeof(PropertyArgumentWithNameInArgumentAttribute), Name, "")]
        [InlineData(full, typeof(PropertyArgumentWithDescriptionAttribute), PropertyArgName, Description)]
        [InlineData(full, typeof(PropertyArgumentWithDescriptionInArgumentAttribute), PropertyArgName, Description)]
        [InlineData(standard, typeof(PropertyArgumentWithName), Name, "")]
        [InlineData(standard, typeof(PropertyArgumentWithNameInArgumentAttribute), Name, "")]
        [InlineData(standard, typeof(PropertyArgumentWithDescriptionInArgumentAttribute), PropertyArgName, Description)]
        public void ArgumentNameAndDescriptionFromProperty(string useStrategy, Type typeToTest, string name, string description)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveName(name)
                    .And.HaveDescription(description);
        }

        [Theory]
        [InlineData(full, typeof(PropertyArgumentWithOneAliasAttribute), AliasAsStringSingle)]
        [InlineData(full, typeof(PropertyArgumentWithThreeAliasesInOneAttribute), AliasAsStringMultiple)]
        public void ArgumentAliasesFromProperty(string useStrategy, Type typeToTest, string aliasesAsString)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(',').Select(s => s.Trim()).ToArray();

            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveAliases(aliases);
        }

        [Theory]
        [InlineData(full, typeof(PropertyArgumentWithOneAllowedValueAttribute), AllowedValuesAsIntFirst)]
        [InlineData(full, typeof(PropertyArgumentWithThreeAllowedValuesInOneAttribute),
                            AllowedValuesAsIntFirst, AllowedValuesAsIntSecond, AllowedValuesAsIntThird)]
        [InlineData(standard, typeof(PropertyArgumentWithOneAllowedValueAttribute), AllowedValuesAsIntFirst)]
        [InlineData(standard, typeof(PropertyArgumentWithThreeAllowedValuesInOneAttribute),
                            AllowedValuesAsIntFirst, AllowedValuesAsIntSecond, AllowedValuesAsIntThird)]
        public void ArgumentAllowedValuesAsFromProperty(string useStrategy, Type typeToTest, params object[] allowedValues)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveAllowedValues(allowedValues);
        }

        [Theory]
        [InlineData(full, typeof(PropertyArgumentWithIsHiddenTrueInArgumentAttribute), true)]
        [InlineData(full, typeof(PropertyArgumentWithIsHiddenFalseInArgumentAttribute), false)]
        [InlineData(full, typeof(PropertyArgumentWithIsHiddenTrue), true)]
        [InlineData(full, typeof(PropertyArgumentWithIsHiddenFalse), false)]
        [InlineData(full, typeof(PropertyArgumentWithIsHiddenTrueAsImplied), true)]
        [InlineData(standard, typeof(PropertyArgumentWithIsHiddenTrueInArgumentAttribute), true)]
        [InlineData(standard, typeof(PropertyArgumentWithIsHiddenFalseInArgumentAttribute), false)]
        public void ArgumentIsHiddenFromProperty(string useStrategy, Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveIsHidden(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(PropertyArgumentWithRequiredTrueInArgumentAttribute), true)]
        [InlineData(full, typeof(PropertyArgumentWithRequiredFalseInArgumentAttribute), false)]
        [InlineData(full, typeof(PropertyArgumentWithRequiredTrue), true)]
        [InlineData(full, typeof(PropertyArgumentWithRequiredFalse), false)]
        [InlineData(full, typeof(PropertyArgumentWithRequiredTrueAsImplied), true)]
        [InlineData(standard, typeof(PropertyArgumentWithRequiredTrueInArgumentAttribute), true)]
        [InlineData(standard, typeof(PropertyArgumentWithRequiredFalseInArgumentAttribute), false)]
        public void ArgumentRequiredFromType(string useStrategy, Type typeToTest, bool isHidden)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveRequired(isHidden);
        }

        [Theory]
        [InlineData(full, typeof(PropertyArgumentWithNoArity), false, 0, 0)]
        [InlineData(full, typeof(PropertyArgumentWithArityLowerBoundOnly), true, 2, int.MaxValue)]
        [InlineData(full, typeof(PropertyArgumentWithArityBothBounds), true, 2, 3)]
        [InlineData(standard, typeof(PropertyArgumentWithNoArity), false, 0, 0)]
        [InlineData(standard, typeof(PropertyArgumentWithArityLowerBoundOnly), true, 2, int.MaxValue)]
        [InlineData(standard, typeof(PropertyArgumentWithArityBothBounds), true, 2, 3)]
        public void ArgumentArityFromType(string useStrategy, Type typeToTest, bool isSet, int minCount, int maxCount)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveArity(isSet, minCount, maxCount);
        }

        [Theory]
        [InlineData(full, typeof(PropertyArgumentWithNoDefaultValue), false, null)]
        [InlineData(full, typeof(PropertyArgumentWithStringDefaultValue), true, DefaultValueString)]
        [InlineData(full, typeof(PropertyArgumentWithIntegerDefaultValue), true, DefaultValueInt)]
        [InlineData(standard, typeof(PropertyArgumentWithNoDefaultValue), false, null)]
        [InlineData(standard, typeof(PropertyArgumentWithStringDefaultValue), true, DefaultValueString)]
        [InlineData(standard, typeof(PropertyArgumentWithIntegerDefaultValue), true, DefaultValueInt)]
        public void ArgumentDefaultValuesFromType(string useStrategy, Type typeToTest, bool isSet, object value)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(useStrategy == full ? fullStrategy : standardStrategy, typeToTest);

            descriptor.Arguments.First()
                    .Should().HaveDefaultValue(isSet, value);
        }

        [Fact]
        public void PropertyOnBaseAndChildIsArgumentOnlyOnParentCommand()
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(Strategy.Standard, typeof(PropertyOnBaseOnlyOnParentCommand));

            descriptor.Arguments.First()
                    .Should().HaveName("Prop");
            descriptor.SubCommands.Should().BeEmpty();
        }
        #endregion

    }
}

