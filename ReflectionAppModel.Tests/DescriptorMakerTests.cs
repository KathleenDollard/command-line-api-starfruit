using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Tests;
using Xunit;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.Linq;
using FluentAssertions;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests.ModelCodeForTests;

namespace System.CommandLine.ReflectionAppModel.Tests
{

    public class DescriptorTests
    {

        private readonly Strategy strategy;

        private const string Method = "Method";
        private const string Type = "Type";

        public DescriptorTests()
        {
            strategy = new Strategy()
                            .SetReflectionRules()
                            .SymbolCandidateNamesToIgnore("CommandDataFromMethods", "CommandDataFromType");
        }

        [Theory]
        [ClassData(typeof(EmptyCommand))]
        [ClassData(typeof(CommandWithDescription))]
        [ClassData(typeof(CommandWithSpecifiedName))]
        [ClassData(typeof(CommandWithTreatUnmatchedTokensAsErrors))]
        [ClassData(typeof(CommandWithIsHidden))]
        [ClassData(typeof(CommandWithOneAlias))]
        [ClassData(typeof(CommandWithMultipleAliases))]
        [ClassData(typeof(CommandWithOneArg))]
        [ClassData(typeof(CommandWithOneOption))]
        //[ClassData(typeof(CommandWithOneSubCommand))]

        [ClassData(typeof(OptionWithSpecifiedName))]
        [ClassData(typeof(OptionWithDescription))]
        [ClassData(typeof(OptionWithIsHidden))]
        [ClassData(typeof(OptionWithRequired))]
        [ClassData(typeof(OptionWithOneAlias))]
        [ClassData(typeof(OptionWithMultipleAliases))]
        [ClassData(typeof(ArgumentWithSpecifiedName))]
        [ClassData(typeof(ArgumentWithDescription))]
        [ClassData(typeof(ArgumentWithIsHidden))]
        [ClassData(typeof(ArgumentWithRequired))]
        [ClassData(typeof(ArgumentWithNonStringArgumentType))]
        [ClassData(typeof(ArgumentWithArity))]
        [ClassData(typeof(ArgumentWithDefaultValue))]
        [ClassData(typeof(ArgumentWithOneAlias))]
        [ClassData(typeof(ArgumentWithMultipleAliases))]
        [ClassData(typeof(OptionWithOneArgument))]
        [ClassData(typeof(OptionArgumentWithSpecifiedName))]
        [ClassData(typeof(OptionArgumentWithDescription))]
        [ClassData(typeof(OptionArgumentWithIsHidden))]
        [ClassData(typeof(OptionArgumentWithRequired))]
        [ClassData(typeof(OptionArgumentWithNonStringArgumentType))]
        public void CommandViaClassData(string id, ClassData.For forSource, ClassData.CommandData commandData)
        {
            RunCommandTests(id, forSource, commandData);
        }

        [Theory]
        [ClassData(typeof(ArgumentWithSpecifiedName))]
        public void CommandInProcess(string id, ClassData.For forSource, ClassData.CommandData commandData)
        {
            RunCommandTests(id, forSource, commandData);
        }

        [Theory]
        [ClassData(typeof(CommandWithOneSubCommand), Skip ="Need ComplexType parameter evaluation")]
        public void CommandViaClassDataBrokenTests(string id, ClassData.For forSource, ClassData.CommandData commandData)
        {
            RunCommandTests(id, forSource, commandData);
        }

        /// <summary>
        /// The Visual Studio test runner groups xUnit ClassData tests as one test. This is
        /// a pain when doing new tests. So I created the ability to group tests for no reason other
        /// than to make VS Test Runner use easier. Yeah, I know.
        /// </summary>
        /// <param name="forSource"></param>
        /// <param name="commandData"></param>
        private void RunCommandTests(string id, ClassData.For forSource, ClassData.CommandData commandData)
        {
            var actual = forSource switch
            {
                ClassData.ForMethod _ => ReflectionDescriptorMaker.RootCommandDescriptor(strategy, forSource.Type.GetMethodsOnDeclaredType().First()),
                ClassData.ForType t => ReflectionDescriptorMaker.RootCommandDescriptor(strategy, t.Type),
                _ => throw new InvalidOperationException("Unexpected data source for test")
            };

            var altName = forSource switch
            {
                ClassData.ForMethod m => m.MethodName,
                ClassData.ForType t => t.Type.Name,
                _ => "<Invalid Name>"
            };

            actual.Should().BeSameAs(commandData.WithAltName(altName).WithId(id));
        }

    }
}