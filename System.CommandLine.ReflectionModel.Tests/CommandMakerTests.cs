using FluentAssertions;
using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace System.CommandLine.ReflectionModel.Tests
{
    public class CommandMakerTests
    {
        private readonly CommandMaker commandMaker;

        public CommandMakerTests()
        {
            commandMaker = new CommandMaker();
            commandMaker.UseDefaults();
        }

        [Fact]
        public void Can_make_argument_for_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "A")
                                .Single();
            var argument = commandMaker.BuildArgument(parameter);
            argument.Should().NotBeNull();
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
            argument.Should().NotBeNull();
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
            option.Should().NotBeNull();
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
            option.Should().NotBeNull();
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
            command.Should().NotBeNull();
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
            command.Should().NotBeNull();
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
            argument.Should().NotBeNull();
            argument.Arity.Should().NotBeNull();
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
            argument.Should().NotBeNull();
            argument.Arity.Should().NotBeNull();
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
            option.Should().NotBeNull();
            option.Argument.Should().NotBeNull();
            option.Argument.Arity.Should().NotBeNull();
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
            option.Should().NotBeNull();
            option.Argument.Should().NotBeNull();
            option.Argument.Arity.Should().NotBeNull();
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
            command.Should().NotBeNull();
            command.Options.Should().HaveCount(4);
            command.Arguments.Should().HaveCount(5);
            command.OfType<Command>().Should().HaveCount(2);
        }

        [Fact]
        public void Add_arguments_options_and_subCommands_for_type()
        {
            var command = new Command("SeaHawks");
            var type = typeof(SampleClass);
            commandMaker.AddChildren(command, type);
            command.Should().NotBeNull();
            command.Options.Count().Should().Be(3);
            command.Arguments.Count().Should().Be(4);
            command.OfType<Command>().Count().Should().Be(2);
        }

        [Fact]
        public void Configure_from_method_command()
        {
            var command = new Command("SeaHawks");
            var method = typeof(CommandMakerTests).GetMethod("SampleMethod");
            commandMaker.Configure(command, method);
            command.Should().NotBeNull();
            command.Options.Count().Should().Be(4);
            command.Arguments.Count().Should().Be(5);
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
            Assert.Throws<ArgumentNullException>("methodInfo", () => commandMaker.Configure(command, (MethodInfo)null));
        }

        [Fact]
        public void Configure_from_type_command_from_type()
        {
            var command = new Command("SeaHawks");
            var type = typeof(SampleClass);
            commandMaker.Configure(command, type);
            command.Options.Should().HaveCount(3);
            command.Arguments.Should().HaveCount(4);
            command.OfType<Command>().Should().HaveCount(2);
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

        [Fact]
        public void Arity_is_default_arity_if_not_required_and_no_arity_on_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "G")
                                .Single();
            var option = commandMaker.BuildOption(parameter);
            option.Should().NotBeNull();
            option.Argument.Should().NotBeNull();
            option.Argument.Arity.Should().NotBeNull();
            option.Argument.Arity.MinimumNumberOfValues.Should().Be(1);
            option.Argument.Arity.MaximumNumberOfValues.Should().Be(1);
        }

        [Fact]
        public void Arity_is_1_1_on_argument_if_required_and_not_collection_on_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "E")
                                .Single();
            var argument = commandMaker.BuildArgument(parameter);
            argument.Should().NotBeNull();
            argument.Arity.Should().NotBeNull();
            argument.Arity.MinimumNumberOfValues.Should().Be(1);
            argument.Arity.MaximumNumberOfValues.Should().Be(1);
        }


        [Fact]
        public void Arity_is_1_1_on_Option_if_required_and_not_collection_on_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "E")
                                .Single();
            var option = commandMaker.BuildOption(parameter);
            option.Should().NotBeNull();
            option.Argument.Should().NotBeNull();
            option.Argument.Arity.Should().NotBeNull();
            option.Argument.Arity.MinimumNumberOfValues.Should().Be(1);
            option.Argument.Arity.MaximumNumberOfValues.Should().Be(1);
        }

        [Fact]
        public void Required_is_true_on_Option_if_required_on_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                .GetMethod("SampleMethod")
                                .GetParameters()
                                .Where(p => p.Name == "D")
                                .Single();
            var option = commandMaker.BuildOption(parameter);
            option.Required.Should().BeTrue();
        }

        [Fact]
        public void Arity_is_1_Max_if_required_and_collection_on_parameter()
        {
            var parameter = typeof(CommandMakerTests)
                                  .GetMethod("SampleMethod")
                                  .GetParameters()
                                  .Where(p => p.Name == "I")
                                  .Single();
            var argument = commandMaker.BuildArgument(parameter);
            argument.Should().NotBeNull();
            argument.Arity.Should().NotBeNull();
            argument.Arity.MinimumNumberOfValues.Should().Be(1);
            argument.Arity.MaximumNumberOfValues.Should().Be(byte.MaxValue);
        }

        [Fact]
        public void Arity_is_default_arity_if_not_required_and_no_arity_on_property()
        {
            var property = typeof(SampleClass)
                                .GetProperties()
                                .Where(p => p.Name == "D")
                                .Single();
            var option = commandMaker.BuildOption(property);
            option.Should().NotBeNull();
            option.Argument.Should().NotBeNull();
            option.Argument.Arity.Should().NotBeNull();
            option.Argument.Arity.MinimumNumberOfValues.Should().Be(1);
            option.Argument.Arity.MaximumNumberOfValues.Should().Be(1);
        }

        [Fact]
        public void Arity_is_1_1_on_option_arguement_if_required_and_not_collection_on_property()
        {
            var property = typeof(SampleClass)
                                    .GetProperties()
                                    .Where(p => p.Name == "E")
                                    .Single();
            var option = commandMaker.BuildOption(property);
            option.Should().NotBeNull();
            option.Argument.Should().NotBeNull();
            option.Argument.Arity.Should().NotBeNull();
            option.Argument.Arity.MinimumNumberOfValues.Should().Be(1);
            option.Argument.Arity.MaximumNumberOfValues.Should().Be(1);
        }


        [Fact]
        public void Arity_is_1_1_on_argument_if_required_and_not_collection_on_property()
        {
            var property = typeof(SampleClass)
                                    .GetProperties()
                                    .Where(p => p.Name == "F")
                                    .Single();
            var argument = commandMaker.BuildArgument(property);
            argument.Should().NotBeNull();
            argument.Arity.Should().NotBeNull();
            argument.Arity.MinimumNumberOfValues.Should().Be(1);
            argument.Arity.MaximumNumberOfValues.Should().Be(1);
        }


        [Fact]
        public void Required_is_true_on_Option_if_required_on_property()
        {
            var property = typeof(SampleClass)
                                  .GetProperties()
                                  .Where(p => p.Name == "F")
                                  .Single();
            var option = commandMaker.BuildOption(property);
            option.Required.Should().BeTrue();
        }

        [Fact]
        public void Arity_min_is_not_reset_if_larger_than_1_and_required_on_property()
        {
            var property = typeof(SampleClass)
                                .GetProperties()
                                .Where(p => p.Name == "C")
                                .Single();
            var option = commandMaker.BuildOption(property);
            option.Should().NotBeNull();
            option.Argument.Should().NotBeNull();
            option.Argument.Arity.Should().NotBeNull();
            option.Argument.Arity.MinimumNumberOfValues.Should().Be(3);
            option.Argument.Arity.MaximumNumberOfValues.Should().Be(4);
        }


        [Fact]
        public void Arity_min_is_correct_if_1_and_required_on_property()
        {
            var property = typeof(SampleClass)
                                .GetProperties()
                                .Where(p => p.Name == "G")
                                .Single();
            var option = commandMaker.BuildOption(property);
            option.Should().NotBeNull();
            option.Argument.Should().NotBeNull();
            option.Argument.Arity.Should().NotBeNull();
            option.Argument.Arity.MinimumNumberOfValues.Should().Be(1);
            option.Argument.Arity.MaximumNumberOfValues.Should().Be(5);
        }

        [Fact]
        public void Derived_classes_appear_as_subCommands()
        {
            var command = new Command("SeaHawks");
            commandMaker.Configure(command, typeof(BaseClass));
            command.Children.OfType<Command>().Should().HaveCount(2);
        }

        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as theory", Justification = "It isn't a theory")]
        public void SampleMethod([CmdArgument] [CmdArity(MinArgCount = 1, MaxArgCount = 3)][Description("Fred")]string A,
                                 string BArgument,
                                 [Description("George")]string C,
                                 [CmdCommand] string A2,
                                 string BCommand,
                                 [CmdOption(ArgumentRequired =true, OptionRequired =true)]
                                 string D,
                                 [CmdArgument(Required = true)]
                                 string E,
                                 [CmdOption(ArgumentRequired =false, OptionRequired =false)]
                                 string F,
                                 [CmdArgument]
                                 string G,
                                 string H,
                                 [CmdArgument(Required = true)]
                                 string[] I)
        { }


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
            [CmdOption(ArgumentRequired = true)]
            [Description("George")]
            public string C { get; set; }
            public string D { get; set; }
            [CmdOption(ArgumentRequired = true, OptionRequired = true)]
            public string E { get; set; }
            [CmdArgument(Required = true)]
            public string F { get; set; }
            [CmdArgument(Required = true)]
            [CmdArity(MinArgCount = 1, MaxArgCount = 5)]
            public string G { get; set; }

        }

        public class BaseClass
        {

        }

        public class DerivedOne :BaseClass
        {

        }

        public class DerivedTwo : BaseClass
        {

        }

    }
}
