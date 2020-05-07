using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class MakerOptionAssertions : ReferenceTypeAssertions<Option, MakerOptionAssertions>
    {
        public MakerOptionAssertions(Option instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "commandFromMaker";

        public AndConstraint<MakerOptionAssertions> MatchesDescriptor(
            OptionDescriptor descriptor, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!(Subject is null))
                .FailWith("Option should not be null")
                .Then
                .ForCondition(!(descriptor is null))
                .FailWith("Descriptor should not be null")
                .Then
                .Given(() => CompareWithDescriptor(Subject, descriptor))
                .ForCondition(issues => !issues.Any())
                .FailWith("Option did not match descriptor. There were {count} issues: {issueList}",
                    issues => issues.Count(), issues => string.Join("/n/l", issues.ToArray()));

            return new AndConstraint<MakerOptionAssertions>(this);
        }

        private static IEnumerable<string> CompareWithDescriptor(Option subject, OptionDescriptor descriptor)
        {
            var format = "{0} {1} does not equal {2}";
            var list = new List<string>();
            if (subject.Name != descriptor.Name) list.Add(string.Format(format, "Name", subject.Name, descriptor.Name));
            if (subject.Description != descriptor.Description) list.Add(string.Format(format, "Description", subject.Description, descriptor.Description));
            if (subject.IsHidden != descriptor.IsHidden) list.Add(string.Format(format, "IsHidden", subject.IsHidden, descriptor.IsHidden));
            var aliasCheck = subject.Aliases.CompareLists(descriptor.Aliases.Prepend(descriptor.Name), "Aliases");
            if (!(aliasCheck is null)) list.Add(aliasCheck);
            return list;
        }

        internal static IEnumerable<string> CompareListWithDescriptor(IEnumerable<Option> commandOptions, List<OptionDescriptor> descriptorOptions)
        {
            var list = new List<string>();
            foreach (var descriptorOption in descriptorOptions)
            {
                var commandOption = commandOptions.Where(a => a.Name == descriptorOption.Name).FirstOrDefault();
                if (commandOption == null)
                {
                    list.Add($"Option {descriptorOption.Name} not found on command");
                }
                list.AddRange(CompareWithDescriptor(commandOption, descriptorOption));
            }
            return list;
        }
    }
}
