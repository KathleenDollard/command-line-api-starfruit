using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class OptionAssertions :
        ReferenceTypeAssertions<Option, OptionAssertions>
    {
        public OptionAssertions(Option instance)
        {
            Subject = instance;
        }
        protected override string Identifier => "systemcommandlinecommand";

        public AndConstraint<OptionAssertions> HaveName(string expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.Name == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Option, "Name", expected, Subject.Name));

            return new AndConstraint<OptionAssertions>(this);
        }

        public AndConstraint<OptionAssertions> HaveDescription(string expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.Description == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Option, "Description", expected, Subject.Description));

            return new AndConstraint<OptionAssertions>(this);
        }

        public AndConstraint<OptionAssertions> HaveIsHidden(bool expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.IsHidden == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Option, "IsHidden", expected, Subject.IsHidden));

            return new AndConstraint<OptionAssertions>(this);
        }

        public AndConstraint<OptionAssertions> HaveAliases(string[] expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Utils.CompareDistinctEnumerable(expected, Subject.Aliases))
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Option, "Aliases", expected, Subject.Aliases));

            return new AndConstraint<OptionAssertions>(this);
        }

        public AndConstraint<OptionAssertions> HaveRequired(bool expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.Required == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Argument, "TreatUnmatchedTokensAsErrors", expected, Subject.Required));

            return new AndConstraint<OptionAssertions>(this);
        }
    }
}
