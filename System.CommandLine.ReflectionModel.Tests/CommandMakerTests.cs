using FluentAssertions;
using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionAppModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace System.CommandLine.ReflectionModel.Tests
{
    public class CommandMakerTests
    {
        private readonly CommandMaker commandMaker;

        public CommandMakerTests()
            => commandMaker = new CommandMaker().UseDefaults();

        [Fact]
        public void Can_make_argument_for_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "A")
                                .Single();
            var argument = commandMaker.BuildArgument(parameter);
            argument.Name.Should().Be("A");
            argument.Description.Should().Be("Fred");
        }

        [Fact]
        public void Can_make_argument_for_property()
        {
            var property = typeof(SampleClass)
                                    .GetProperties()
                                    .Where(p => p.Name == "A")
                                    .Single();
            var argument = commandMaker.BuildArgument(property);
            argument.Name.Should().Be("A");
            argument.Description.Should().Be("Fred");
        }


        [Fact]
        public void Can_make_option_for_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "C")
                                .Single();
            var option = commandMaker.BuildOption(parameter);
            option.Name.Should().Be("c");
            option.Description.Should().Be("George");
        }

        [Fact]
        public void Can_make_option_for_property()
        {
            var property = typeof(SampleClass)
                                    .GetProperties()
                                    .Where(p => p.Name == "C")
                                    .Single();
            var option = commandMaker.BuildOption(property);
            option.Name.Should().Be("c");
            option.Description.Should().Be("George");
        }

        [Fact]
        public void Can_make_command_for_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "A")
                                .Single();
            var command = commandMaker.BuildCommand(parameter);
            command.Name.Should().Be("a");
            command.Description.Should().Be("Fred");
        }

        [Fact]
        public void Can_make_command_for_property()
        {
            var property = typeof(SampleClass)
                                    .GetProperties()
                                    .Where(p => p.Name == "A")
                                    .Single();
            var command = commandMaker.BuildCommand(property);
            command.Name.Should().Be("a");
            command.Description.Should().Be("Fred");
        }

        [Fact]
        public void Can_make_complex_argument_for_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "A")
                                .Single();
            var argument = commandMaker.BuildArgument(parameter);
            argument.Name.Should().Be("A");
            argument.Description.Should().Be("Fred");
            argument.Arity.MinimumNumberOfValues.Should().Be(1);
            argument.Arity.MaximumNumberOfValues.Should().Be(3);
        }

        [Fact]
        public void Can_make_complex_argument_for_property()
        {
            var property = typeof(SampleClass)
                                    .GetProperties()
                                    .Where(p => p.Name == "A")
                                    .Single();
            var argument = commandMaker.BuildArgument(property);
            argument.Name.Should().Be("A");
            argument.Description.Should().Be("Fred");
            argument.Arity.MinimumNumberOfValues.Should().Be(1);
            argument.Arity.MaximumNumberOfValues.Should().Be(3);
        }

        [Fact]
        public void Can_make_complex_option_for_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "A")
                                .Single();
            var option = commandMaker.BuildOption(parameter);
            option.Name.Should().Be("a");
            option.Description.Should().Be("Fred");
            option.Argument.Arity.MinimumNumberOfValues.Should().Be(1);
            option.Argument.Arity.MaximumNumberOfValues.Should().Be(3);
        }

        [Fact]
        public void Can_make_complex_option_for_property()
        {
            var property = typeof(SampleClass)
                                    .GetProperties()
                                    .Where(p => p.Name == "C")
                                    .Single();
            var option = commandMaker.BuildOption(property);
            option.Name.Should().Be("c");
            option.Description.Should().Be("George");
            option.Argument.Arity.MinimumNumberOfValues.Should().Be(3);
            option.Argument.Arity.MaximumNumberOfValues.Should().Be(4);
        }

        [Fact]
        public void Add_arguments_options_and_subCommands_for_method()
        {
            var command = new Command("SeaHawks");
            var method = typeof(CommandMakerTests).GetMethod("SampleMethod");
            commandMaker.AddChildren(command, method);
            command.Options.Count().Should().Be(1);
            command.Arguments.Count().Should().Be(2);
            command.OfType<Command>().Count().Should().Be(2);
        }

        [Fact]
        public void Add_arguments_options_and_subCommands_for_type()
        {
            var command = new Command("SeaHawks");
            var type = typeof(SampleClass);
            commandMaker.AddChildren(command, type);
            command.Options.Count().Should().Be(2);
            command.Arguments.Count().Should().Be(2);
            command.OfType<Command>().Count().Should().Be(2);
        }

        [Fact]
        public void Configure_from_method_command()
        {
            var command = new Command("SeaHawks");
            var method = typeof(CommandMakerTests).GetMethod("SampleMethod");
            commandMaker.Configure(command, method);
            command.Options.Count().Should().Be(1);
            command.Arguments.Count().Should().Be(2);
            command.OfType<Command>().Count().Should().Be(2);
        }

        [Fact]
        public void Configure_from_method_fails_on_null_command()
        {
            var method = typeof(CommandMakerTests).GetMethod("SampleMethod");
            Assert.Throws<ArgumentNullException>("command", () => commandMaker.Configure(null, method));
        }

        [Fact]
        public void Configure_from_method_fails_on_null_method()
        {
            var command = new Command("SeaHawks");
            Assert.Throws<ArgumentNullException>("method",() => commandMaker.Configure(command, (MethodInfo)null));
        }

        [Fact]
        public void Configure_from_type_command_from_type()
        {
            var command = new Command("SeaHawks");
            var type = typeof(SampleClass);
            commandMaker.Configure(command, type);
            command.Options.Count().Should().Be(2);
            command.Arguments.Count().Should().Be(2);
            command.OfType<Command>().Count().Should().Be(2);
        }

        [Fact]
        public void Configure_from_type_fails_on_null_command()
        {
            var type = typeof(SampleClass);
            Assert.Throws<ArgumentNullException>("command", () => commandMaker.Configure(null, type));
        }

        [Fact]
        public void Configure_from_type_fails_on_null_type()
        {
            var command = new Command("SeaHawks");
            Assert.Throws<ArgumentNullException>("type", () => commandMaker.Configure(command, (Type)null));
        }

        public void SampleMethod([CmdArgument] [CmdArity(MinArgCount = 1, MaxArgCount = 3)][Description("Fred")]string A,
                                 string BArgument,
                                 [Description("George")]string C,
                                 [CmdCommand] string A2,
                                 string BCommand) { }

        public class SampleClass
        {
            [CmdArgument]
            [CmdArity(MinArgCount = 1, MaxArgCount = 3)]
            [Description("Fred")]
            public string A { get; set; }
            [CmdCommand]
            public string A2 { get; set; }
            public string BArgument { get; set; }
            public string BCommand { get; set; }
            [CmdArity(MinArgCount = 3, MaxArgCount = 4)]
            [Description("George")]
            public string C { get; set; }
            public string D { get; set; }
        }

    }
}
