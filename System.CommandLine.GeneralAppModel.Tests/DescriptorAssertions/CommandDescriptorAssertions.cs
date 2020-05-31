﻿using FluentAssertions;
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
    public static class CommandDescriptorTestExtensions
    {
        public static CommandDescriptorAssertions Should(this CommandDescriptor instance)
        {
            return new CommandDescriptorAssertions(instance);
        }

        public static CommandDescriptorAssertions Should(this ObjectAssertions assertions)
        {
            return (CommandDescriptorAssertions)assertions.Subject;
        }
    }

    public class CommandDescriptorAssertions : SymbolDescriptorAssertions<CommandDescriptor, CommandDescriptorAssertions>
    {
        public CommandDescriptorAssertions(CommandDescriptor instance)
            : base(instance)
        { }

        protected override string Identifier => "commanddescriptor";

        public AndConstraint<CommandDescriptorAssertions> HaveTreatUnmatchedTokensAsErrors(bool expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.TreatUnmatchedTokensAsErrors == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "TreatUnmatchedTokensAsErrors", expected, Subject.TreatUnmatchedTokensAsErrors));

            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveSubCommandsNamed(IEnumerable<string> expected)
        {
            var actual = string.Join(",", Subject.SubCommands.Select(sub => sub.Name));
            var expectedString = string.Join(",", expected);
            Execute.Assertion
                     .ForCondition(actual == expectedString)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "SubCommands", expectedString, actual));

            return new AndConstraint<CommandDescriptorAssertions>(this);

        }

        public AndConstraint<CommandDescriptorAssertions> HaveOptionsNamed(IEnumerable<string> expected)
        {
            var actual = string.Join(",", Subject.Options.Select(sub => sub.Name));
            var expectedString = string.Join(",", expected);
            Execute.Assertion
                     .ForCondition(actual == expectedString)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Options", expectedString, actual));

            return new AndConstraint<CommandDescriptorAssertions>(this);

        }

        public AndConstraint<CommandDescriptorAssertions> HaveArgumentsNamed(IEnumerable<string> expected)
        {
            var actual = string.Join(",",Subject.Arguments.Select(sub => sub.Name));
            var expectedString = string.Join(",", expected);
            Execute.Assertion
                     .ForCondition(actual == expectedString)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Arguments", expectedString, actual));

            return new AndConstraint<CommandDescriptorAssertions>(this);

        }

    }
}

