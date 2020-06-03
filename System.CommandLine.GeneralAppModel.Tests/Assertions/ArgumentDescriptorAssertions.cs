using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class ArgumentDescriptorTestExtensions
    {
        public static ArgumentDescriptorAssertions Should(this ArgumentDescriptor instance)
        {
            return new ArgumentDescriptorAssertions(instance);
        }

    }

    public class ArgumentDescriptorAssertions : SymbolDescriptorAssertions<ArgumentDescriptor, ArgumentDescriptorAssertions>
    {
        public ArgumentDescriptorAssertions(ArgumentDescriptor instance)
            : base(instance)
        { }

        protected override string Identifier => "commanddescriptor2";

        public AndConstraint<ArgumentDescriptorAssertions> HaveArity(bool isSet, int minValue, int maxValue)
        {
            Execute.Assertion
                 .ForCondition(!isSet ? Subject.Arity is null : true)
                 .FailWith("Expected there not to be an Arity, but found one")
                 .Then
                 .ForCondition(isSet ? !(Subject.Arity is null) : true)
                 .FailWith("Expected to be an Arity, but did not find one");

            if (isSet && !(Subject.Arity is null))
            {
                Execute.Assertion
                    .ForCondition(minValue == Subject.Arity.MinimumCount &&
                               maxValue == Subject.Arity.MaximumCount)
                    .FailWith($"Expected Arity to be {minValue} to {maxValue}, " +
                            $"but found {Subject.Arity.MinimumCount} to {Subject.Arity.MaximumCount}");
            }

            return new AndConstraint<ArgumentDescriptorAssertions>(this);
        }

        public AndConstraint<ArgumentDescriptorAssertions> HaveDefaultValue(bool isSet, object defaultValue)
        {
            // Be careful simplifying the following. VS wants to you to, but bugs and a lack of clarity resulted. 
            Execute.Assertion
                 .ForCondition(isSet ? !(Subject.DefaultValue is null) : true)
                 .FailWith("Expected the default value to be set, but found it was not")
                 .Then
                 .ForCondition(!isSet ? Subject.DefaultValue is null : true)
                 .FailWith("Expected the default value not to be set, but found that it was set");

            if (!(defaultValue is null) && !(Subject.DefaultValue is null))
            {
                Execute.Assertion
                     .ForCondition(Equals(Subject.DefaultValue.DefaultValue, defaultValue))
                     .FailWith($"Expected DefaultValue to be {defaultValue}, but found {(isSet ? Subject.DefaultValue.DefaultValue : "<wat?>")}");
            }

            return new AndConstraint<ArgumentDescriptorAssertions>(this);
        }

        public AndConstraint<ArgumentDescriptorAssertions> HaveAllowedValues(IEnumerable<object>? expected)
        {
            var actualAllowedValues = Subject.AllowedValues;
            if (actualAllowedValues is null && expected is null)
            {
                return new AndConstraint<ArgumentDescriptorAssertions>(this);
            }

            var expectedDisplay = expected is null
                               ? string.Empty
                               : string.Join(",", expected);
            var actualDisplay = actualAllowedValues is null
                               ? string.Empty
                               : string.Join(",", actualAllowedValues);

            if (expected is null || actualAllowedValues is null)
            {
                Execute.Assertion
                              .ForCondition(expected is null)
                              .FailWith($"Allowed values  were not expected, but found {expectedDisplay}.")
                              .Then
                              .ForCondition(!(expected is null))
                              .FailWith($"Allowed values were expected, but were not found. Expected {actualDisplay}");
                return new AndConstraint<ArgumentDescriptorAssertions>(this);
            }
            var match = true;
            foreach (var value in expected)
            {
                if (!actualAllowedValues.Contains(value))
                {
                    match = false;
                }
            }
                Execute.Assertion
                             .ForCondition(match)
                             .FailWith($"Expected {expectedDisplay}, but found {actualDisplay }.");

            return new AndConstraint<ArgumentDescriptorAssertions>(this);
        }

        public AndConstraint<ArgumentDescriptorAssertions> HaveArgumentType(Type expected)
        {
            Type actual = Subject.ArgumentType.GetArgumentType<Type>();
            Execute.Assertion
                 .ForCondition(actual == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Argument, "ArgumentType", expected, actual));

            return new AndConstraint<ArgumentDescriptorAssertions>(this);
        }

        public AndConstraint<ArgumentDescriptorAssertions> HaveRequired(bool expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.Required == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Required", expected, Subject.Required));

            return new AndConstraint<ArgumentDescriptorAssertions>(this);
        }
    }
}

