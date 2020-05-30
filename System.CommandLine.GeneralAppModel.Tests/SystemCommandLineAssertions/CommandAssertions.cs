using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class CommandAssertions :
        ReferenceTypeAssertions<Command, CommandAssertions>
    {
        public CommandAssertions(Command instance)
        {
            Subject = instance;
        }
        protected override string Identifier => "systemcommandlinecommand";

        public AndConstraint<CommandAssertions> HaveName(string expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.Name == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Name", expected, Subject.Name));

            return new AndConstraint<CommandAssertions>(this);
        }

        public AndConstraint<CommandAssertions> HaveDescription(string expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.Description == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Description", expected, Subject.Description));

            return new AndConstraint<CommandAssertions>(this);
        }

        public AndConstraint<CommandAssertions> HaveIsHidden(bool expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.IsHidden == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "IsHidden", expected, Subject.IsHidden));

            return new AndConstraint<CommandAssertions>(this);
        }

        public AndConstraint<CommandAssertions> HaveAliases(string[] expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Utils.CompareDistinctEnumerable(expected, Subject.Aliases))
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Aliases", expected, Subject.Aliases));

            return new AndConstraint<CommandAssertions>(this);
        }

        public AndConstraint<CommandAssertions> HaveTreatUnmatchedTokensAsErrors(bool expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.TreatUnmatchedTokensAsErrors == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "TreatUnmatchedTokensAsErrors", expected, Subject.TreatUnmatchedTokensAsErrors));

            return new AndConstraint<CommandAssertions>(this);
        }
    }
}
