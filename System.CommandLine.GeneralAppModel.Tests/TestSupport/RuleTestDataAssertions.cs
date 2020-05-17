using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class RuleTestDataExtensions
    {
        public static RuleTestDataAssertions Should(this RuleBase instance)
        {
            return new RuleTestDataAssertions(instance);
        }
    }

    public class RuleTestDataAssertions :
            ReferenceTypeAssertions<RuleBase, RuleTestDataAssertions>
    {
        public RuleTestDataAssertions(RuleBase instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "commanddescriptor";

        public AndConstraint<RuleTestDataAssertions> Match(
            RuleBase expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition((Subject & expected) == expected)
                .FailWith($"SymbolType {expected} expected, but {Subject} found");

            return new AndConstraint<RuleTestDataAssertions>(this);
        }
    }
}