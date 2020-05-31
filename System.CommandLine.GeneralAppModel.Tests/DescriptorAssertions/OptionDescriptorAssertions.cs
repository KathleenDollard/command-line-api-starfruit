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
    public static class OptionDescriptorTestExtensions2
    {
        public static OptionDescriptorAssertions Should(this OptionDescriptor instance)
        {
            return new OptionDescriptorAssertions(instance);
        }

    }

    public class OptionDescriptorAssertions : SymbolDescriptorAssertions<OptionDescriptor, OptionDescriptorAssertions>
    {
        public OptionDescriptorAssertions(OptionDescriptor instance)
            : base(instance)
        { }

        protected override string Identifier => "commanddescriptor2";

        public AndConstraint<OptionDescriptorAssertions> HaveRequired(bool expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.Required == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Required", expected, Subject.Required));

            return new AndConstraint<OptionDescriptorAssertions>(this);
        }

        public AndConstraint<OptionDescriptorAssertions> HaveArgumentsNamed(IEnumerable<string> expected)
        {
            var actual = Subject.Arguments.Select(sub => sub.Name);
            Execute.Assertion
                     .ForCondition(actual == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Arguments", expected, actual));

            return new AndConstraint<OptionDescriptorAssertions>(this);

        }
    }
}

