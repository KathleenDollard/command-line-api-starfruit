using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class ArgumentAssertions :
        ReferenceTypeAssertions<Argument, ArgumentAssertions>
    {
        public ArgumentAssertions(Argument instance)
        {
            Subject = instance;
        }
        protected override string Identifier => "systemcommandlinecommand";

        public AndConstraint<ArgumentAssertions> HaveName(string expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.Name == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Argument, "Name", expected, Subject.Name));

            return new AndConstraint<ArgumentAssertions>(this);
        }

        public AndConstraint<ArgumentAssertions> HaveDescription(string expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.Description == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Argument, "Description", expected, Subject.Description));

            return new AndConstraint<ArgumentAssertions>(this);
        }

        public AndConstraint<ArgumentAssertions> HaveIsHidden(bool expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.IsHidden == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Argument, "IsHidden", expected, Subject.IsHidden));

            return new AndConstraint<ArgumentAssertions>(this);
        }

        public AndConstraint<ArgumentAssertions> HaveAliases(string[] expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Utils.CompareDistinctEnumerable(expected, Subject.Aliases, nullAndEmptyRelaxed: true))
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Argument, "Aliases", expected, Subject.Aliases));

            return new AndConstraint<ArgumentAssertions>(this);
        }

        public AndConstraint<ArgumentAssertions> HaveArity(bool isSet, int? minValue, int? maxValue, string because = "", string becauseArgs = "")
        {
            if (Subject.Arity is null)
            {
                Execute.Assertion
                       .ForCondition(!isSet)
                       .FailWith("Expected there not to be an Arity, but did not find one");
                return new AndConstraint<ArgumentAssertions>(this);

            }

            if (!isSet)
            {
                Execute.Assertion
                        .ForCondition(Subject.Arity is null)
                        .FailWith("Expected there not to be an Arity, but found one");
                return new AndConstraint<ArgumentAssertions>(this);
            }
            Execute.Assertion
                        .ForCondition(minValue == Subject.Arity.MinimumNumberOfValues &&
                               maxValue == Subject.Arity.MaximumNumberOfValues)
                        .FailWith($"Expected Arity to be {minValue} to {maxValue}, " +
                            $"but found {Subject.Arity.MinimumNumberOfValues} to {Subject.Arity.MaximumNumberOfValues}");

            return new AndConstraint<ArgumentAssertions>(this);
        }

        public AndConstraint<ArgumentAssertions> HaveDefaultValue(bool isSet, object defaultValue, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(!isSet ? !Subject.HasDefaultValue : true)
                 .FailWith("Expected the default value not to be set, but found that it was set")
                 .Then
                 .ForCondition(isSet ? Subject.HasDefaultValue : true)
                 .FailWith("Expected the default value to be set, but found it was not")
                 .Then
                 .ForCondition(isSet ? Equals(Subject.GetDefaultValue(),defaultValue) : true)
                 .FailWith($"Expected DefaultValue to be {defaultValue}, but found {(isSet ? Subject.GetDefaultValue() : "<wat?>")}");

            return new AndConstraint<ArgumentAssertions>(this);
        }


        public AndConstraint<ArgumentAssertions> HaveArgumentType(Type expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.ArgumentType == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Argument, "TreatUnmatchedTokensAsErrors", expected, Subject.ArgumentType));

            return new AndConstraint<ArgumentAssertions>(this);
        }

        public AndConstraint<ArgumentAssertions> HaveHasDefaultValue(bool expected, string because = "", string becauseArgs = "")
        {
            Execute.Assertion
                 .ForCondition(Subject.HasDefaultValue == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Argument, "TreatUnmatchedTokensAsErrors", expected, Subject.HasDefaultValue));

            return new AndConstraint<ArgumentAssertions>(this);
        }
    }
}
