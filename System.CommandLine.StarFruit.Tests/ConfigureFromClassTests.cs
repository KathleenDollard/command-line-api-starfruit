using FluentAssertions;
using System.CommandLine;
using System.CommandLine.ReflectionModel;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace System.CommandLine.StarFruit.Tests
{
    public class ConfigureFromClassTests
    {
        private readonly ITestOutputHelper _output;

        public ConfigureFromClassTests(ITestOutputHelper output)
            => _output = output;

        [Fact]
        public void RootCommand_created_when_isRoot_true()
        {
            var command = (new ReflectionParser<BaseClass>()).GetCommand(true);
            command.Should().BeOfType<RootCommand>();
        }

        [Fact]
        public void Command_created_when_isRoot_default()
        {
            var command = (new ReflectionParser<BaseClass>()).GetCommand();
            command.Should().BeOfType<Command>();
        }

        [Fact]
        public void Command_name_and_alias_from_class_name()
        {
            var command = (new ReflectionParser<SimpleEmptyClass>()).GetCommand();
            command.Name.Should().Be("simple-empty-class");
            command.Aliases.Should().BeEquivalentTo(new[] { "simple-empty-class" });
        }

        [Fact]
        public void Command_hiearchy_name_and_alias_correct()
        {
            var command = (new ReflectionParser<EmptyBaseClass>()).GetCommand();
            command.Name.Should().Be("empty-base-class");
            command.Aliases.Should().BeEquivalentTo(new[] { "empty-base-class" });
            command.Children.Count().Should().Be(1);
            var child = command.Children.First();
            child.Should().BeOfType<Command>();
            child.Name.Should().Be("child-class-of-empty");
            child.Aliases.Should().BeEquivalentTo(new[] { "child-class-of-empty" });
        }

        [Fact]
        public void Options_from_class_definition_correct()
        {
            var command = (new ReflectionParser<ChildClassOfEmpty>()).GetCommand();
            command.Options.Count().Should().Be(1);
            var option = command.Options.Single();
            option.Name.Should().Be("greeting");
            option.Aliases.Should().BeEquivalentTo(new[] { "greeting" });
            option.Argument.ArgumentType.Should().Be(typeof(string));
            option.Argument.Name.Should().Be("greeting");
        }

        [Fact]
        public void Options_from_base_class_definition_correct()
        {
            var command = (new ReflectionParser<BaseClass>()).GetCommand();
            command.Options.Count().Should().Be(3);
            var option = command.Options.First();
            CheckOption(command.Options.ToArray()[0], "name", typeof(string));
            CheckOption(command.Options.ToArray()[1], "count", typeof(int));
            CheckOption(command.Options.ToArray()[2], "uppercase", typeof(bool));
        }

        [Fact]
        public void Argument_from_base_class_definition_correct()
        {
            var command = (new ReflectionParser<BaseClass>()).GetCommand();
            command.Arguments.Count().Should().Be(1);
            CheckArgument(command.Arguments.First(), "birthday-greeting", typeof(string));
        }

        [Fact]
        public void Argument_with_collection_type_on_base_correct()
        {
            var command = (new ReflectionParser<CollecionArgBase>()).GetCommand();
            command.Arguments.Count().Should().Be(1);
            CheckArgument(command.Arguments.First(), "parent-names", typeof(string[]));
        }

        [Fact]
        public void Argument_with_collection_type_on_child_correct()
        {
            var command = (new ReflectionParser<CollecionArgBase>()).GetCommand();
            var subCommand = command.Children.OfType<Command>().Single();
            subCommand.Arguments.Count().Should().Be(1);
            CheckArgument(subCommand.Arguments.First(), "child-names", typeof(string[]));
        }

        [Fact]
        public void Argument_arity_from_attribute()
        {
            var command = (new ReflectionParser<Details>()).GetCommand();
            command.Arguments.Count().Should().Be(1);
            command.Arguments.First().Arity.MinimumNumberOfValues.Should().Be(2);
            command.Arguments.First().Arity.MaximumNumberOfValues.Should().Be(3);
        }

        [Fact]
        public void Argument_default_from_attribute()
        {
            var command = (new ReflectionParser<Details>()).GetCommand();
            command.Arguments.Count().Should().Be(1);
            command.Arguments.First().GetDefaultValue().Should().BeEquivalentTo(new[] { "Pizza" });
        }

        [Fact]
        public void Option_with_collection_argument_on_base_correct()
        {
            var command = (new ReflectionParser<CollecionArgBase>()).GetCommand();
            command.Options.Count().Should().Be(1);
            CheckOption(command.Options.First(), "parent-ages", typeof(int[]));
        }

        [Fact]
        public void Option_with_collection_argument_on_child_correct()
        {
            var command = (new ReflectionParser<CollecionArgBase>()).GetCommand();
            var subCommand = command.Children.OfType<Command>().Single();
            subCommand.Options.Count().Should().Be(1);
            CheckOption(subCommand.Options.First(), "child-ages", typeof(int[]));
        }

        [Fact]
        public void Option_arity_from_attribute()
        {
            var command = (new ReflectionParser<Details>()).GetCommand();
            command.Options.Count().Should().Be(1);
            var option = command.Options.First();
            option.Argument.Arity.MaximumNumberOfValues.Should().Be(1);
            option.Argument.Arity.MinimumNumberOfValues.Should().Be(1);
            CheckOption(option, "fave-year", typeof(int));
        }

        [Fact]
        public void Option_default_from_attribute()
        {
            var command = (new ReflectionParser<Details>()).GetCommand();
            command.Options.Count().Should().Be(1);
            var option = command.Options.First();
            option.Argument.GetDefaultValue().Should().Be(2001);
            CheckOption(option, "fave-year", typeof(int));
        }

        private void CheckArgument(Argument argument, string name, Type type)
        {
            argument.Name.Should().Be(name);
            argument.ArgumentType.Should().Be(type);
        }

        private static void CheckOption(Option option, string name, Type type)
        {
            option.Name.Should().Be(name);
            option.Argument.Name.Should().Be(name);
            option.Argument.ArgumentType.Should().Be(type);
        }

        public class SimpleEmptyClass
        { }

        public class EmptyBaseClass
        { }

        public class ChildClassOfEmpty : EmptyBaseClass
        {
            public string Greeting { get; set; }
        }

        public class BaseClass
        {
            [CmdArgument(DefaultValue = "Older")]
            public string BirthdayGreeting { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }
            public bool Uppercase { get; set; }
        }

        public class ChildClass : BaseClass
        {
            public string Greeting { get; set; }
            [CmdArgument(DefaultValue = "Yo")]
            public string Farewell { get; set; }
        }

        [CmdCommand(Description = "This is a command", Name = "Details")]
        public class Details
        {
            [CmdArgument(Description = "Your favorite foods", DefaultValue = "Pizza")]
            [CmdArity(MinArgCount = 2, MaxArgCount = 3)]
            public string[] FaveFoods { get; set; }

            [CmdOption(Description = "Your favorite year", DefaultValue = 2001)]
            [CmdRange(1950, 2020)]
            [CmdArity(MinArgCount = 1)]
            public int FaveYear { get; set; }
        }


        [CmdCommand(Description = "This is a command", Name = "Details")]
        public class Details2
        {
            [CmdOption(Description = "Your favorite year", OptionRequired = true, ArgumentRequired = true)]
            [CmdRange(1950, 2020)]
            public int FaveYear { get; set; }

        }

        public class CollecionArgBase
        {
            [CmdArgument]
            public string[] ParentNames { get; set; }
            public int[] ParentAges { get; set; }
        }

        public class CollectionArgChild : CollecionArgBase
        {
            [CmdArgument]
            public string[] ChildNames { get; set; }
            public int[] ChildAges { get; set; }
        }
    }
}
