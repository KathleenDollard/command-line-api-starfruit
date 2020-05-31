using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class DescriptorValidationAssertionsExtensions
    {
        public static DescriptorValidationAssertions Should(this DescriptorInvalidException instance)
        {
            return new DescriptorValidationAssertions(instance);
        }
    }

    public class DescriptorValidationAssertions : ReferenceTypeAssertions<DescriptorInvalidException, DescriptorValidationAssertions>
    {
        public DescriptorValidationAssertions(DescriptorInvalidException instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "descriptorinvalidexception";

        public AndConstraint<DescriptorValidationAssertions> HaveFailures(params (string id, string path)[] allExpected)
        {
            using var scope = new AssertionScope();
            Subject.ValidationFailures.Should().HaveCount(allExpected.Count());
            foreach (var (expectedId, expectedPath) in allExpected)
            {
                var actualFound = Subject.ValidationFailures.Where(x => x.Path == expectedPath);
                if (!actualFound.Any())
                {
                    Execute.Assertion
                       .FailWith($"Expected to find an issue with the path '{expectedPath}', but did not find one");
                    continue;
                }
                var actual = actualFound.First();
                Execute.Assertion
                   .ForCondition(actual.Id == expectedId)
                   .FailWith($"Expected problem '{expectedId}', but found problem '{actual.Id}'");
            }
            return new AndConstraint<DescriptorValidationAssertions>(this);
        }
    }
}
