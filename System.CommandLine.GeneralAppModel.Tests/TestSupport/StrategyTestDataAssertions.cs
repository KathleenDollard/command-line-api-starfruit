using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests.TestSupport
{
    public static class RuleGroupExtensions
    {
        public static RuleGroupAssertions Should(this RuleGroupBase instance)
        {
            return new RuleGroupAssertions(instance);
        }
    }

    public class RuleGroupAssertions :
         ReferenceTypeAssertions<RuleGroupBase, RuleGroupAssertions>
    {
        public RuleGroupAssertions(RuleGroupBase instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "commanddescriptor";

        public AndConstraint<RuleGroupBase> BeEquivalentTo(
            RuleGroupTestData expected, string because = "", params object[] becauseArgs)
        {
            var message = "";
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(CheckAllRules(Subject.Rules, expected.Rules, ref message))
                .FailWith($"Rules didn't match");

            return new AndConstraint<RuleGroupBase>(this);
        }

        private bool CheckAllRules(IEnumerable<RuleBase> actual,IEnumerable<RuleBaseTestData >expected, ref string message)
        {
            var actualArray = actual.ToArray();
            var expectedArray = expected.ToArray();
            for (int i = 0; i < actualArray.Length; i++)
            {
                actualArray[i].Should().Match(expectedArray[i]);
            }
        }

        private bool CheckEachRule(object rule)
        {
            var 
        }
    }
}

