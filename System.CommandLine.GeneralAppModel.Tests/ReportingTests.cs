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
            var expected = "The identity, usually the name.";

            actual.Should().Be(expected);
        }

        [Fact(Skip = "")]
        public void ReportForLabelRuleIsCorrect()
        {
        }

        [Theory]
        [InlineData("Abc", @"the name ends with 'Abc'")]
        public void ReportForNameRuleForGetItemsIsCorrect(string compareTo, string expected)
        {
            var rule = new NameEndsWithRule(compareTo);
            var actual = rule.RuleDescription<IRuleGetCandidates>();

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("Abc", @"If name ends with 'Abc', remove 'Abc'")]
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

            actual.Should().Be(@"If there is an attribute named 'DescriptionAttribute'");
        }

        [Fact]
        public void ReportForNamedAttributeWithPropertyRuleIsCorrect()
        {
            var rule = new AttributeWithPropertyValueRule<ArgumentAttribute, string>( "Name");
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be("If there is an attribute named 'ArgumentAttribute', its 'Name' property, with type System.String");
        }

        [Fact()]
        public void ReportForRemainingSymbolRuleIsCorrect()
        {
            var rule = new RemainingSymbolRule(SymbolType.Argument);
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be("not already matched");
        }

        [Theory]
        [InlineData(StringContentsRule.StringPosition.BeginsWith, "Abc", @"the string begins with 'Abc'")]
        public void ReportForStringContentxForGetItemsIsCorrect(StringContentsRule.StringPosition position,
            string compareTo, string expected)
        {
            var rule = new StringContentsRule(position, compareTo);
            var actual = rule.RuleDescription<IRuleGetCandidates>();

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(StringContentsRule.StringPosition.BeginsWith, "Abc", @"If string begins with 'Abc', remove 'Abc'")]
        public void ReportForStringContentsForGetValueIsCorrect(StringContentsRule.StringPosition position,
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
    }
}
