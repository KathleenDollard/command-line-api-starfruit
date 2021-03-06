﻿using Xunit;
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
                            .SetFullRules()
                            .SymbolCandidateNamesToIgnore("CommandDataFromMethods", "CommandDataFromType");
        }

        [Fact]
        public void StrategyNameIsCorrect()
        {
            generalStrategy.Name.Should().Be(generalStrategyName);
        }

        [Fact]
        public void GeneralStrategyGetCandidateRulesIsCorrect()
        {
            var rules = generalStrategy.GetCandidateRules.Rules.ToArray();
            rules[0].CheckDerivedFromRule(null, null, false);
        }

        [Fact]
        public void GeneralStrategSelectSymbolRulesIsCorrect()
        {
            var rules = generalStrategy.SelectSymbolRules.Rules.ToArray();

            rules.Should().HaveCount(7);
            rules[0].CheckAttributeRule<CommandAttribute>(SymbolType.Command);
            rules[1].CheckNamePatternRule(SymbolType.Command, StringPosition.EndsWith, "Command");
            rules[2].CheckIsOfTypeRule(SymbolType.Command, typeof(Type));
            rules[3].CheckAttributeRule<ArgumentAttribute>(SymbolType.Argument);
            rules[4].CheckNamePatternRule(SymbolType.Argument, StringPosition.EndsWith, "Argument");
            rules[5].CheckNamePatternRule(SymbolType.Argument, StringPosition.EndsWith, "Arg");
            rules[6].CheckRule<RemainingSymbolRule>(SymbolType.Option);

        }


        [Fact(Skip = "Will probably remove these rules or rewrite. 2 is correct (current failure)")]
        public void GeneralStrategyArgumentRulesIsCorrect()
        {

            var descriptionRules = generalStrategy.ArgumentRules.DescriptionRules.Rules.ToArray();
            descriptionRules.CheckRules(new RuleGroupTestData
            {
                SymbolType = SymbolType.Argument,
                Rules = new List<RuleBaseTestData>
                    {
                        new NamedAttributeWithPropertyTestData("Description","Description", typeof(string)),
                        new NamedAttributeWithPropertyTestData("Argument","Description", typeof(string)),
                    }
            });

            var nameRules = generalStrategy.ArgumentRules.NameRules.Rules.ToArray();
            nameRules.CheckRules(new RuleGroupTestData
            {
                SymbolType = SymbolType.Argument,
                Rules = new List<RuleBaseTestData>
                    {
                        new NamedAttributeWithPropertyTestData("Name","Name", typeof(string)),
                        new NamedAttributeWithPropertyTestData("Argument","Name", typeof(string)),
                        new NamePatternTestData(StringPosition.EndsWith, "Arg"),
                        new NamePatternTestData(StringPosition.EndsWith, "Argument"),
                        new IdentityRuleTestData(typeof(string))
                    }
            });

            var isHiddenRules = generalStrategy.ArgumentRules.IsHiddenRules.Rules.ToArray();
            isHiddenRules.CheckRules(new RuleGroupTestData
            {
                SymbolType = SymbolType.Argument,
                Rules = new List<RuleBaseTestData>
                    {
                        new NamedAttributeTestData("Hidden"),
                    }
            });


            var requiredRules = generalStrategy.ArgumentRules.RequiredRules.Rules.ToArray();
            requiredRules.CheckRules(new RuleGroupTestData
            {
                SymbolType = SymbolType.Argument,
                Rules = new List<RuleBaseTestData>
                    {
                        new NamedAttributeTestData("Required"),
                        new NamedAttributeWithPropertyTestData("Argument","Required", typeof(string)),
                    }
            });
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
