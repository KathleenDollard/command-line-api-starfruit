﻿using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Xunit.Sdk;
using static System.CommandLine.GeneralAppModel.Tests.ModelCodeForTests.ClassData;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class ArgumentDescriptorTestExtensions2
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

        public AndConstraint<ArgumentDescriptorAssertions> BeEmptyExcept(params string[] skipChecks)
        {
            return new AndConstraint<ArgumentDescriptorAssertions>(this);
        }

        public AndConstraint<ArgumentDescriptorAssertions> HaveArity(bool isSet, int? minValue, int? maxValue)
        {
            if (isSet)
            {
                var _ = !minValue.HasValue || !maxValue.HasValue
                       ? throw new InvalidOperationException("MinValue and MaxValue must be set when IsSet is true. For no maxValue, use Int32.Max")
                       : 0;
            }
            Execute.Assertion
                 .ForCondition(!isSet ? Subject.Arity is null : true)
                 .FailWith("Expected there not to be an Arity, but found one")
                 .Then
                 .ForCondition(isSet ? !(Subject.Arity is null) : true)
                 .FailWith("Expected to be an Arity, but did not find one");

            if (isSet && !(Subject.Arity is null))
            {
                Execute.Assertion
                    .ForCondition(minValue.Value == Subject.Arity.MinimumCount &&
                               maxValue.Value == Subject.Arity.MaximumCount)
                    .FailWith($"Expected Arity to be {minValue.Value} to {maxValue.Value}, " +
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
                     .ForCondition(Subject.DefaultValue.DefaultValue.Equals(defaultValue))
                     .FailWith($"Expected DefaultValue to be {defaultValue}, but found {(isSet ? Subject.DefaultValue.DefaultValue : "<wat?>")}");
            }

            return new AndConstraint<ArgumentDescriptorAssertions>(this);
        }


        public AndConstraint<ArgumentDescriptorAssertions> HaveArgumentType(Type expected)
        {
            Execute.Assertion
                 .ForCondition(Subject.ArgumentType == expected)
                 .FailWith(Utils.DisplayEqualsFailure(SymbolType.Argument, "ArgumentType", expected, Subject.ArgumentType));

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

