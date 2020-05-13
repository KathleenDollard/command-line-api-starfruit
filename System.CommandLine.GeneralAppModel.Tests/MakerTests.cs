using FluentAssertions;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Tests;
using Xunit;

namespace System.CommandLine.ReflectionModel.Tests
{

    public class MakerTests
    {
        [Fact]
        public void CanMakeSimpleCommand()
        {
            var descriptor = TestData.CommandData1.CreateDescriptor();
            var command = CommandMaker.MakeCommand(descriptor);

            var expected = TestData.CommandData1.CreateCommand();
            command.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void CanMakeSimpleOption()
        {
            var descriptor = TestData.OptionData1.CreateDescriptor(null);
            var option = CommandMaker.MakeOption(descriptor);

            var expected = TestData.OptionData1.CreateOption();
            option.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void CanMakeSimpleArgument()
        {
            var descriptor = TestData.ArgData1.CreateDescriptor(null);
            var argument = CommandMaker.MakeArgument(descriptor);

            var expectedArg = TestData.ArgData1.CreateArgument();
            argument.Should().BeEquivalentTo(expectedArg);
        }

        [Fact]
        public void CommandsHasCorrectArgument()
        {
            var descriptor = TestData.CommandData1.CreateDescriptor();
            descriptor.Arguments.Add(TestData.ArgData1.CreateDescriptor(null));
            var command = CommandMaker.MakeCommand(descriptor);

            var expected = TestData.CommandData1.CreateCommand();
            command.Should().BeEquivalentTo(expected);
        }

    }
}