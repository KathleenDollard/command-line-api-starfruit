using System.CommandLine.Collections;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests.Maker;
using System.Data;
using Xunit;

namespace System.CommandLine.ReflectionModel.Tests.Maker
{

    public class MakerTests
    {
        // Constants to make tests quicker to write
        private readonly string name1 = "Fred";
        private readonly string desc1 = "This is a description";
        private readonly string[] aliases = { "a", "b", "c" };
        private readonly Type argumentType = typeof(string);

        [Fact]
        public void CanMakeSimpleCommand()
        {
            var descriptor = new CommandDescriptor
            {
                Name = name1,
                Description = desc1,
                IsHidden = true,
                Aliases = aliases
            };
            var command = CommandMaker.MakeCommand(descriptor);
            command.Should().MatchesDescriptor(descriptor);
        }

        [Fact]
        public void CanMakeSimpleOption()
        {
            var descriptor = new OptionDescriptor
            {
                Name = name1,
                Description = desc1,
                IsHidden = true,
                Aliases = aliases
            };
            var option = CommandMaker.MakeOption(descriptor);
            option.Should().MatchesDescriptor(descriptor);
        }

        [Fact]
        public void CanMakeSimpleArgument()
        {
            var descriptor = GetArgumentDescriptor(name1, desc1, argumentType, true, aliases);
            var argument = CommandMaker.MakeArgument(descriptor);
            argument.Should().MatchesDescriptor(descriptor);
        }

        [Fact]
        public void CommandsHasCorrectArgument()
        {
            var descriptor = new CommandDescriptor
            {
                Name = name1,
            };
            descriptor.Arguments.Add(GetArgumentDescriptor("arg1", desc1, argumentType, true, aliases));
            var command = CommandMaker.MakeCommand(descriptor);

            var expectedCommand = MapToCommand(descriptor);
            command.Should().BeEquivalentTo(expectedCommand);
            // command.Should().MatchesDescriptor(descriptor);
        }

        private Command MapToCommand(CommandDescriptor descriptor)
        {

            var format = "{0} {1} does not equal {2}";
            var list = new List<string>();
            if (subject.Name != descriptor.Name) list.Add(string.Format(format, "Name", subject.Name, descriptor.Name));
            if (subject.Description != descriptor.Description) list.Add(string.Format(format, "Description", subject.Description, descriptor.Description));
            if (subject.IsHidden != descriptor.IsHidden) list.Add(string.Format(format, "IsHidden", subject.IsHidden, descriptor.IsHidden));

            List<string> expectedAliases = new List<string> { descriptor.Name };
            if (!(descriptor.Aliases is null))
            {
                expectedAliases.AddRange(descriptor.Aliases);
            }
            var aliasCheck = subject.Aliases.CompareLists(expectedAliases, "Aliases");
            if (!(aliasCheck is null)) list.Add(aliasCheck);

            var argumentsCheck = MakerArgumentAssertions.CompareListWithDescriptor(subject.Arguments, descriptor.Arguments);
            if (argumentsCheck.Any()) list.Add(String.Join("/t/n/l", argumentsCheck));

            var optionsCheck = MakerOptionAssertions.CompareListWithDescriptor(subject.Options, descriptor.Options);
            if (optionsCheck.Any()) list.Add(String.Join("/t/n/l", optionsCheck));

            var subCommandsCheck = MakerCommandAssertions.CompareListWithDescriptor(subject.Children.OfType<Command>(), descriptor.SubCommands);
            if (subCommandsCheck.Any()) list.Add(String.Join("/t/n/l", subCommandsCheck));
        }

        private ArgumentDescriptor GetArgumentDescriptor(string name, string description, Type argumentType, bool isHidden, string[] aliases)
        {
            return new ArgumentDescriptor
            {
                Name = name,
                Description = description,
                IsHidden = isHidden,
                Aliases = aliases,
                ArgumentType = argumentType
            };
        }
    }
}