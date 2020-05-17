using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class SymbolTypeTestExtensions
    {
        public static SymbolTypeAssertions Should(this SymbolType instance)
        {
            return new SymbolTypeAssertions(instance);
        }
    }

    public class SymbolTypeAssertions :
            ReferenceTypeAssertions<SymbolType, SymbolTypeAssertions>
    {
        public SymbolTypeAssertions(SymbolType instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "commanddescriptor";

        public AndConstraint<SymbolTypeAssertions> IncludeSymbolType(
            SymbolType expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition((Subject & expected) == expected)
                .FailWith($"SymbolType {expected} expected, but {Subject} found");

            return new AndConstraint<SymbolTypeAssertions>(this);
        }
    }
}