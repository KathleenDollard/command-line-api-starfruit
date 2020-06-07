using FluentAssertions;
using FluentAssertions.Equivalency;
using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using Xunit;

namespace System.CommandLine.NamedAttributeRules.Tests
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

        [Theory]
        [InlineData("NamedAttribute", @"If there is an attribute named 'NamedAttribute'")]
        public void ReportForNamedAttributeRuleIsCorrect(string attributeName, string expected)
        {
            var rule = new NamedAttributeRule(attributeName);
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("WithProperty", "ThisProperty", @"If there is an attribute named 'WithProperty', its 'ThisProperty' property, with type System.String")]
        public void ReportForNamedAttributeWithPropertyRuleIsCorrect(string attributeName, string propertyName, string expected)
        {
            var rule = new NamedAttributeWithPropertyValueRule<string>(attributeName, propertyName);
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be(expected);
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

        [Theory]
        [InlineData("Abc", "Def", typeof(int), "Ghi", typeof(string), "If there is an attribute named 'Abc': Def as System.Int32, Ghi as System.String")]
        public void ReportForComplexAttributeRuleGetValueIsCorrect(string attributeName,
                    string propName1, Type type1, string propName2, Type type2, string expected)
        {
            var rule = new NamedAttributeWithComplexValueRule(attributeName)
            {
            };
            rule.PropertyNamesAndTypes.Add(new NamedAttributeWithComplexValueRule.NameAndType(propName1, propName1, propertyType: type1));
            rule.PropertyNamesAndTypes.Add(new NamedAttributeWithComplexValueRule.NameAndType(propName2, propName2, propertyType: type2));
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("Abc", "Def", "If there is an attribute named 'Abc' with a property 'Def', inlcude it as a Int32")]
        public void ReportForOptionalValueAttributeRuleGetValueIsCorrectForInts(string attributeName,
            string propName1, string expected)
        {
            var rule = new NamedAttributeWithOptionalValueRule<int>(attributeName, propName1);
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
