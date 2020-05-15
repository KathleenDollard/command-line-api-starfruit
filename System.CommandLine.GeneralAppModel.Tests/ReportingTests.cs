using FluentAssertions;
using FluentAssertions.Equivalency;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class ReportingTests
    {
        private static Func<EquivalencyAssertionOptions<Symbol>, EquivalencyAssertionOptions<Symbol>> symbolOptions;
        private readonly Strategy strategy;


        public ReportingTests()
        {
            AssertionOptions.AssertEquivalencyUsing(o => o.ExcludingFields().IgnoringCyclicReferences());
            //argumentOptions = o => o.Excluding(ctx => ctx.SelectedMemberInfo.MemberName =="TryConvertArgument");
            symbolOptions = o => o.Excluding(ctx => ctx.SelectedMemberPath.EndsWith("ConvertArguments"));
            strategy = new Strategy().SetStandardRules();
        }

        //[Theory]
        //[InlineData(StringContentsRule.StringPosition.BeginsWith, "Abc", @"if the name begins with ""Abc""")]
        //public void ReportForBoolAttributeRuleIsCorrect(string attributeName, )
        //{
        //}

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
        [InlineData(StringContentsRule.StringPosition.BeginsWith, "Abc", @"if the name begins with ""Abc""")]
        public void ReportForNameRuleForGetItemsIsCorrect(StringContentsRule.StringPosition position,
            string compareTo, string expected)
        {
            var rule = new NamePatternRule(position, compareTo);
            var actual = rule.RuleDescription<IRuleGetItems>();

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(StringContentsRule.StringPosition.BeginsWith, "Abc", @"If name begins with ""Abc"", remove ""Abc""")]
        public void ReportForNameRuleForGetValueIsCorrect(StringContentsRule.StringPosition position,
            string compareTo, string expected)
        {
            var rule = new NamePatternRule(position, compareTo);
            var actual = rule.RuleDescription<IRuleGetValue<string>>();

            actual.Should().Be(expected);
        }

        [Fact(Skip = "")]
        public void ReportForNamedAttributeRuleIsCorrect()
        {
        }

        [Fact(Skip = "")]
        public void ReportForRemainingSymbolRuleIsCorrect()
        {
        }

        [Theory]
        [InlineData (StringContentsRule.StringPosition.BeginsWith, "Abc", @"if the string begins with ""Abc""")]
        public void ReportForStringContentxForGetItemsIsCorrect(StringContentsRule.StringPosition position,
            string compareTo, string expected)
        {
            var rule = new StringContentsRule(position , compareTo );
            var actual = rule.RuleDescription< IRuleGetItems>();

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(StringContentsRule.StringPosition.BeginsWith, "Abc", @"If string begins with ""Abc"", remove ""Abc""")]
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
            report.Length.Should().BeGreaterThan(3000).And.BeLessThan(4000);
        }
    }
}
