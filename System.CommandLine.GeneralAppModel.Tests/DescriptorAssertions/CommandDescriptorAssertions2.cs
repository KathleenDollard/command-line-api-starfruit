using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Xunit.Sdk;
using static System.CommandLine.GeneralAppModel.Tests.ModelCodeForTests.ClassData;

namespace System.CommandLine.GeneralAppModel.Tests
{
    //public static class CommandDescriptorTestExtensions2
    //{
    //    public static CommandDescriptorAssertions2 Should(this CommandDescriptor instance)
    //    {
    //        return new CommandDescriptorAssertions2(instance);
    //    }

    //}

    public class CommandDescriptorAssertions2 : SymbolDescriptorAssertions<CommandDescriptor, CommandDescriptorAssertions2>
    {
        public CommandDescriptorAssertions2(CommandDescriptor instance)
            : base(instance)
        { }

        protected override string Identifier => "commanddescriptor2";

        public AndConstraint<CommandDescriptorAssertions2> BeEmptyExcept(params string[] skipChecks)
        {
            return new AndConstraint<CommandDescriptorAssertions2>(this);
        }

        public AndConstraint<CommandDescriptorAssertions2> HaveTreatUnmatchedTokensAsErrors(bool expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.TreatUnmatchedTokensAsErrors == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "TreatUnmatchedTokensAsErrors", expected, Subject.TreatUnmatchedTokensAsErrors));

            return new AndConstraint<CommandDescriptorAssertions2>(this);
        }

        public AndConstraint<CommandDescriptorAssertions2> HaveSubCommandsNamed(IEnumerable<string> expected)
        {
            var actual = Subject.SubCommands.Select(sub => sub.Name);
            Execute.Assertion
                     .ForCondition(actual == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "SubCommands", expected, actual));

            return new AndConstraint<CommandDescriptorAssertions2>(this);

        }

        public AndConstraint<CommandDescriptorAssertions2> HaveOptionsNamed(IEnumerable<string> expected)
        {
            var actual = Subject.Options.Select(sub => sub.Name);
            Execute.Assertion
                     .ForCondition(actual == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Options", expected, actual));

            return new AndConstraint<CommandDescriptorAssertions2>(this);

        }

        public AndConstraint<CommandDescriptorAssertions2> HaveArgumentsNamed(IEnumerable<string> expected)
        {
            var actual = Subject.Arguments.Select(sub => sub.Name);
            Execute.Assertion
                     .ForCondition(actual == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Arguments", expected, actual));

            return new AndConstraint<CommandDescriptorAssertions2>(this);

        }
    }
}

