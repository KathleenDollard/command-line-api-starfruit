using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class MakerArgumentAssertions : ReferenceTypeAssertions<Argument, MakerArgumentAssertions>
    {
        public MakerArgumentAssertions(Argument instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "commandFromMaker";

        public AndConstraint<MakerArgumentAssertions> MatchesDescriptor(
            ArgumentDescriptor descriptor, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!(Subject is null))
                .FailWith("Argument should not be null")
                .Then
                .ForCondition(!(descriptor is null))
                .FailWith("Descriptor should not be null")
                .Then
                .Given(() => CompareWithDescriptor(Subject, descriptor))
                .ForCondition(issues => !issues.Any())
                .FailWith("Argument did not match descriptor. There were {count} issues: {issueList}",
                    issues => issues.Count(), issues => string.Join("/n/l", issues.ToArray()));

            return new AndConstraint<MakerArgumentAssertions>(this);
        }

        private static IEnumerable<string> CompareWithDescriptor(Argument subject, ArgumentDescriptor descriptor)
        {
            var format = "Name {0} does not equal {0}";
            var list = new List<string>();
            if (subject.Name != descriptor.Name) list.Add(string.Format(format, "Name", subject.Name, descriptor.Name));
            if (subject.Description != descriptor.Description) list.Add(string.Format(format, "Description", subject.Description, descriptor.Description));
            if (subject.IsHidden != descriptor.IsHidden) list.Add(string.Format(format, "IsHidden", subject.IsHidden, descriptor.IsHidden));
            if (subject.ArgumentType != descriptor.ArgumentType) list.Add(string.Format(format, "IsHidden", subject.IsHidden, descriptor.IsHidden));
            var aliasCheck = subject.Aliases.CompareLists(descriptor.Aliases, "Aliases");
            if (!(aliasCheck is null)) list.Add(aliasCheck);
            return list;
        }

        internal static IEnumerable<string> CompareListWithDescriptor(IEnumerable<Argument> commandArgs, List<ArgumentDescriptor> descriptorArgs)
        {
            var list = new List<string>();
            foreach (var descriptorArg in descriptorArgs)
            {
                var commandArg = commandArgs.Where(a => a.Name == descriptorArg.Name).FirstOrDefault();
                if (commandArg == null)
                {
                    list.Add( $"Argument {descriptorArg.Name} not found on command" );
                }
                list.AddRange( CompareWithDescriptor(commandArg, descriptorArg));
            }
            return list;
        }
    }
}
