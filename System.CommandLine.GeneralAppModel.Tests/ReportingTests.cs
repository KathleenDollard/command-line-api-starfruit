using FluentAssertions;
using FluentAssertions.Equivalency;
using System;
using System.Collections.Generic;
using Xunit;
using System.CommandLine.GeneralAppModel;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class ReportingTests
    {
        private readonly Strategy strategy;


        public ReportingTests()
        {
            AssertionOptions.AssertEquivalencyUsing(o => o.ExcludingFields().IgnoringCyclicReferences());
            strategy = new Strategy().SetFullRules();
        }

        [Fact(Skip = "")]
        public void ReportForComplexAttributeRuleIsCorrect()
        {
        }

        [Fact]
        public void ReportForIdentityRuleIsCorrect()
        {
            var rule = new IdentityRule<string>();
            var actual = rule.RuleDescription<IRuleGetValues<string>>();
            var expected = "has not been matched, use the identity which is usually the Name";

            actual.Should().Be(expected);
        }

        [Fact(Skip = "")]
        public void ReportForLabelRuleIsCorrect()
        {
        }

        [Theory]
        [InlineData("Abc", @"with a Name that ends with 'Abc'")]
        public void ReportForNameRuleForGetItemsIsCorrect(string compareTo, string expected)
        {
            var rule = new NameEndsWithRule(compareTo);
            var actual = rule.RuleDescription<IRuleGetCandidates>();

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("Abc", @"with a Name that ends with 'Abc'")]
        public void ReportForNameRuleForGetValueIsCorrect(string compareTo, string expected)
        {
            var rule = new NameEndsWithRule(compareTo);
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ReportForNamedAttributeRuleIsCorrect()
        {
            var rule = new AttributeRule<DescriptionAttribute>();
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be(@"with an attribute named 'Description'");
        }

        [Fact]
        public void ReportForNamedAttributeWithPropertyRuleIsCorrect()
        {
            var rule = new AttributeWithPropertyValueRule<ArgumentAttribute, string>( "Name");
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be("has an attribute named 'Argument', use the 'Name' property if present");
        }

        [Fact()]
        public void ReportForRemainingSymbolRuleIsCorrect()
        {
            var rule = new RemainingSymbolRule(SymbolType.Argument);
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be("that is not already matched");
        }

        [Theory]
        [InlineData(StringPosition.BeginsWith, "Abc", @"with a string that begins with 'Abc'")]
        public void ReportForStringContentxForGetItemsIsCorrect(StringPosition position,
            string compareTo, string expected)
        {
            var rule = new StringContentsRule(position, compareTo);
            var actual = rule.RuleDescription<IRuleGetCandidates>();

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(StringPosition.BeginsWith, "Abc", @"with a string that begins with 'Abc'")]
        public void ReportForStringContentsForGetValueIsCorrect(StringPosition position,
            string compareTo, string expected)
        {
            var rule = new StringContentsRule(position, compareTo);
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be(expected);
        }

        [Fact]
        public void FullStrategyReportIsAboutTheRightLength()
        {
            var report = strategy.Report();
            report.Length.Should().BeGreaterThan(4500);
        }

        [Fact]
        public void StandardStrategyReportIsAboutTheRightLength()
        {
            var strategy = new Strategy().SetStandardRules();
            var report = strategy.Report();
            report.Length.Should().BeGreaterThan(4000);
        }
    }
}
