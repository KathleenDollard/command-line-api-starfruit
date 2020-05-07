using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class MakerCommandAssertions : ReferenceTypeAssertions<Command, MakerCommandAssertions>
    {
        public MakerCommandAssertions(Command instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "commandFromMaker";

        public AndConstraint<MakerCommandAssertions> MatchesDescriptor(
            CommandDescriptor descriptor, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!(Subject is null))
                .FailWith("Command should not be null")
                .Then
                .ForCondition(!(descriptor is null))
                .FailWith("Descriptor should not be null")
                .Then
                .Given(() => CompareWithDescriptor(Subject, descriptor))
                .ForCondition(issues => !issues.Any())
                .FailWith("Command did not match descriptor. There were {count} issues: {issueList}",
                    issues => issues.Count(), issues => string.Join("/n/l", issues.ToArray()));

            return new AndConstraint<MakerCommandAssertions>(this);
        }

        private static IEnumerable<string> CompareWithDescriptor(Command subject, CommandDescriptor descriptor)
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

            var subCommandsCheck = MakerCommandAssertions.CompareListWithDescriptor(subject.Children.OfType<Command>(), descriptor.SubCommands );
            if (subCommandsCheck.Any()) list.Add(String.Join("/t/n/l", subCommandsCheck));

            return list;
        }

        internal static IEnumerable<string> CompareListWithDescriptor(IEnumerable<Command> commandCommands, List<CommandDescriptor> descriptorCommands)
        {
            var list = new List<string>();
            foreach (var descriptorCommand in descriptorCommands)
            {
                var commandCommand = commandCommands.Where(a => a.Name == descriptorCommand.Name).FirstOrDefault();
                if (commandCommand == null)
                {
                    list.Add($"Commandument {descriptorCommand.Name} not found on command");
                }
                list.AddRange(CompareWithDescriptor(commandCommand, descriptorCommand));
            }
            return list;
        }

    }
}
