using FluentAssertions;
using FluentAssertions.Equivalency;
using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Tests;
using Xunit;

namespace System.CommandLine.ReflectionAppModel.Tests
{

    public class MakerTests
    {
        private static Func<EquivalencyAssertionOptions<Symbol>, EquivalencyAssertionOptions<Symbol>> symbolOptions;

        public MakerTests()
        {
            AssertionOptions.AssertEquivalencyUsing(o => o.ExcludingFields().IgnoringCyclicReferences());
            //argumentOptions = o => o.Excluding(ctx => ctx.SelectedMemberInfo.MemberName =="TryConvertArgument");
            symbolOptions = o => o.Excluding(ctx => ctx.SelectedMemberPath.EndsWith( "ConvertArguments"));
        }

        [Fact]
        public void CanMakeSimpleCommand()
        {
            var descriptor = TestData.CommandData1.CreateDescriptor();
            var command = CommandMaker.MakeCommand(descriptor);

            var expected = TestData.CommandData1.CreateCommand();
            command.Should().BeEquivalentTo(expected, symbolOptions);
        }

        [Fact]
        public void CanMakeSimpleOption()
        {
            var descriptor = TestData.OptionData1.CreateDescriptor(null);
            var option = CommandMaker.MakeOption(descriptor);

            var expected = TestData.OptionData1.CreateOption();
            option.Should().BeEquivalentTo(expected, symbolOptions);
        }

        [Fact]
        public void CanMakeArgument()
        {
            var descriptor = TestData.ArgData1.CreateDescriptor(null);
            var argument = CommandMaker.MakeArgument(descriptor);

            var expectedArg = TestData.ArgData1.CreateArgument();
            argument.Should().BeEquivalentTo(expectedArg, symbolOptions);
        }

        [Fact]
        public void CommandsHasCorrectArgument()
        {
            var data = TestData.CommandData1;
            data.Arguments = new ArgumentTestData[] { TestData.ArgData1 };
            var descriptor = TestData.CommandData1.CreateDescriptor();
            var command = CommandMaker.MakeCommand(descriptor);

            var expected = TestData.CommandData1.CreateCommand();
            command.Should().BeEquivalentTo(expected, symbolOptions);
        }

    }
}