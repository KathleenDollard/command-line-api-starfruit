using Xunit;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class StrategyCreationTests
    {
        private readonly Strategy generalStrategy;
        private const string generalStrategyName = "General";

        public StrategyCreationTests()
        {
            generalStrategy = new Strategy(generalStrategyName)
                            .SetGeneralRules()
                            .CandidateNamesToIgnore("CommandDataFromMethods", "CommandDataFromType");
        }

        [Fact]
        public void StrategyNameIsCorrect()
        {
            generalStrategy.Name.Should().Be(generalStrategyName);
        }

        [Fact]
        public void GeneralStrategyGetCandidateRulesIsCorrect()
        {
            var rules = generalStrategy.GetCandidateRules.Rules;
            rules.Should().BeEmpty(); // this is set by a non-abstract Strategy
        }

        [Fact]
        public void GeneralStrategSelectSymbolRulesIsCorrect()
        {
            var rules = generalStrategy.SelectSymbolRules.Rules.ToArray();

            rules.Should().HaveCount(6);
            rules[0].CheckNamedAttributeRule(SymbolType.Command, "Command");
            rules[1].CheckNamePatternRule(SymbolType.Command, StringContentsRule.StringPosition.EndsWith, "Command");
            rules[2].CheckNamedAttributeRule(SymbolType.Argument, "Argument");
            rules[3].CheckNamePatternRule(SymbolType.Argument, StringContentsRule.StringPosition.EndsWith, "Argument");
            rules[4].CheckNamePatternRule(SymbolType.Argument, StringContentsRule.StringPosition.EndsWith, "Arg");
            rules[5].CheckRule<RemainingSymbolRule>(SymbolType.Option);

        }


        [Fact]
        public void GeneralStrategyArgumentRulesIsCorrect()
        {
            var descriptionData = new RuleGroupTestData
            {
                SymbolType = SymbolType.Argument,
                Rules = new List<RuleBaseTestData>
                    {
                        new NamedAttributeWithPropertyTestData("Description","Description", typeof(string)),
                        new NamedAttributeWithPropertyTestData("Argument","Description", typeof(string)),
                    }
            };

            var descriptionRules = generalStrategy.ArgumentRules.DescriptionRules.Rules.ToArray();

            CheckRules(descriptionRules, descriptionData);

            static void CheckRules(IRule[] actual, RuleGroupTestData descriptionData)
            {
                var symbolType = descriptionData.SymbolType;
                var expectedRules = descriptionData.Rules.ToArray();
                actual.Should().HaveCount(expectedRules.Length);
                for (int i = 0; i < actual.Length; i++)
                {
                    switch (actual[i])
                    {
                        case NamePatternRule r:
                            var np = expectedRules[i] as NamePatternTestData;
                            r.CheckNamePatternRule(symbolType, np.Position, np.CompareTo);
                            break;
                        case NamedAttributeRule r:
                            var na = expectedRules[i] as NamedAttributeTestData;
                            r.CheckNamedAttributeRule(symbolType, na.AttributeName );
                            break;
                        case NamedAttributeWithPropertyRule<string> r:
                            var naps = expectedRules[i] as NamedAttributeWithPropertyTestData;
                            r.CheckNamedAttributeWithPropertyRule(symbolType, naps.AttributeName, naps.PropertyName , type);
                            break;
                        case NamedAttributeWithPropertyRule<bool> r:
                            var rapb = expectedRules[i] as NamedAttributeWithPropertyTestData;
                            r.CheckNamedAttributeWithPropertyRule(symbolType, rapb.AttributeName, rapb.PropertyName, type);
                            break;

                    }
                }
            }

            descriptionRules.Should().BeSameAs(descriptionData);

            descriptionRules.Should().HaveCount(2);
            descriptionRules[0].CheckNamedAttributeWithPropertyRule<string>(SymbolType.Argument, "Description", "Description");
            descriptionRules[1].CheckNamedAttributeWithPropertyRule<string>(SymbolType.Argument, "Argument", "Description");






            var nameRules = generalStrategy.ArgumentRules.NameRules.Rules.ToArray();
            nameRules.Should().HaveCount(5);
            nameRules[0].CheckNamedAttributeWithPropertyRule<string>(SymbolType.Argument, "Name", "Name");
            nameRules[1].CheckNamedAttributeWithPropertyRule<string>(SymbolType.Argument, "Argument", "Name");
            nameRules[2].CheckNamePatternRule(SymbolType.Argument, StringContentsRule.StringPosition.EndsWith, "Arg");
            nameRules[3].CheckNamePatternRule(SymbolType.Argument, StringContentsRule.StringPosition.EndsWith, "Argument");
            nameRules[4].CheckRule<IdentityRule<string>>(SymbolType.Argument);

            var IsHiddenRules = generalStrategy.ArgumentRules.IsHiddenRules.Rules.ToArray();
            IsHiddenRules.Should().HaveCount(1);
            IsHiddenRules[0].CheckNamedAttributeRule(SymbolType.Argument, "Hidden");

            var requiredRules = generalStrategy.ArgumentRules.RequiredRules.Rules.ToArray();
            requiredRules.Should().HaveCount(2);
            requiredRules[0].CheckNamedAttributeRule(SymbolType.Argument, "Required");
            requiredRules[1].CheckNamedAttributeWithPropertyRule<bool>(SymbolType.Argument, "Argument", "Required");
        }

        [Fact]
        public void GeneralStrategyOptionRulesIsCorrect()
        {
            // var rules = generalStrategy.OptionRules.Rules;
        }

        [Fact]
        public void GeneralStrategyCommandRulesIsCorrect()
        {
            /// var rules = generalStrategy.CommandRules.Rules;
        }
    }
}
