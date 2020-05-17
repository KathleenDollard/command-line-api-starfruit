using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class CommandDescriptorTestExtensions
    {
        // Help! I can't figure out how to have new fluent extension assertions be additive - existing checks stop working when I add a new check.
        public static CommandDescriptorAssertions Should1(this CommandDescriptor instance)
        {
            return new CommandDescriptorAssertions(instance);
        }
    }

    public class CommandDescriptorAssertions :
            ReferenceTypeAssertions<CommandDescriptor, CommandDescriptorAssertions>
    {
        public CommandDescriptorAssertions(CommandDescriptor instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "commanddescriptor";

        public AndConstraint<CommandDescriptorAssertions> CheckSubCommandDescriptors(
            CommandDescriptor expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!(expected is null))
                .FailWith("Expected can't be null")
                .Then
                .ForCondition(Subject.SubCommands.Count() == expected.SubCommands.Count())
                .FailWith($"Count of subCommands expected to be {expected.SubCommands.Count()} and was {Subject.SubCommands.Count()}");

            return new AndConstraint<CommandDescriptorAssertions>(this);
        }
    }
}

