using FluentAssertions;
using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Text;
using System.Threading;
using Xunit;

namespace System.CommandLine.StarFruit.Tests
{
    public class ParseTests
    {
        [Fact]
        public void Value_on_rootcommand_is_correct()
        {
            var command = BaseCommand();
            var result = command.Parse("--count 3");
            result.Errors.Count.Should().Be(0);
        }


        [Fact]
        public void Error_on_rootcommand_is_reported()
        {
            var command = BaseCommand();
            var result = command.Parse("--count Joe" );
            result.Errors.Count.Should().Be(1);
        }


        [Fact]
        public void Error_on_child_command_is_reported()
        {
            var command = BaseChildCommand();
            var result = command.Parse("child-class --count2 Joe");
            result.Errors.Count.Should().Be(1);
        }


        [Fact(Skip = "Issue #767")]
        public void Error_on_parent_command_is_reported()
        {
            var command = BaseChildCommand();
            var result = command.Parse("--count Joe child-class");
            result.Errors.Count.Should().Be(1);
        }

        private Command BaseChildCommand()
        {
            var command = BaseCommand();
            command.AddCommand(ChildCommand());
            return command;
        }

        public Command BaseCommand()
        {
            var command = new Command("base-class");
            var argument = new Argument<string>("birthday-greeting")
            {
                Arity = new ArgumentArity(0, 1)
            };
            argument.SetDefaultValue("Older");
            command.AddArgument(argument);
            command.AddOption(new Option<string>("--name", () => "MyName"));
            command.AddOption(new Option<int>("--count"));

            return command;
        }

        public Command ChildCommand()
        {
            var subCommand = new Command("child-class");
            var argument = new Argument<string>("farewell");
            argument.SetDefaultValue("Yo");
            subCommand.AddArgument(argument);
            subCommand.AddOption(new Option<string>("--greeting"));
            subCommand.AddOption(new Option<int>("--count2"));
            return subCommand;
        }
    }
}
