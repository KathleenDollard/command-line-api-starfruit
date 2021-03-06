﻿using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class SymbolDescriptorAssertions<TDesc, TAssert> : ReferenceTypeAssertions<TDesc, TAssert>
        where TDesc : SymbolDescriptor
        where TAssert : SymbolDescriptorAssertions<TDesc, TAssert>
    {
        public SymbolDescriptorAssertions(TDesc instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "commanddescriptor2";

        public AndConstraint<TAssert> HaveNotNullRawItem()
        {
            Execute.Assertion
               .ForCondition(!(Subject.Raw is null))
               .FailWith("Raw value should not be null");

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public AndConstraint<TAssert> HaveName(string expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.Name == expected)
                     .FailWith(Utils.DisplayEqualsFailure(Subject.SymbolType , "Name", expected, Subject.Name));

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public AndConstraint<TAssert> HaveOriginalName(string expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.OriginalName == expected)
                     .FailWith(Utils.DisplayEqualsFailure(Subject.SymbolType, "OriginalName", expected, Subject.OriginalName));

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public AndConstraint<TAssert> HaveCommandLineName(string expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.CommandLineName == expected)
                     .FailWith(Utils.DisplayEqualsFailure(Subject.SymbolType, "CommandLineName", expected, Subject.CommandLineName));

            return new AndConstraint<TAssert>((TAssert)this);
        }


        public AndConstraint<TAssert> HaveDescription(string expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.Description == expected)
                     .FailWith(Utils.DisplayEqualsFailure(Subject.SymbolType, "Description", expected, Subject.Description));

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public AndConstraint<TAssert> HaveIsHidden(bool expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.IsHidden == expected)
                     .FailWith(Utils.DisplayEqualsFailure(Subject.SymbolType, "IsHidden", expected, Subject.IsHidden));

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public AndConstraint<TAssert> HaveAliases(string[]? expected)
        {
            if (Subject.Aliases is null && expected is null)
            {
                return new AndConstraint<TAssert>((TAssert)this);
            }
            var expectedAliases = expected is null
                                ? string.Empty
                                : string.Join(",", expected);
            var actualAliases = Subject.Aliases is null
                               ? string.Empty
                               : string.Join(",", Subject.Aliases);
            if (expected is null || Subject.Aliases is null)
            {
                Execute.Assertion
                              .ForCondition(expected is null)
                              .FailWith($"Aliases were not expected, but were found: {actualAliases }")
                              .Then
                              .ForCondition(!(expected is null))
                              .FailWith($"Aliases were expected, but were not found. Expected {expectedAliases}");
                return new AndConstraint<TAssert>((TAssert)this);
            }
            Execute.Assertion
                             .ForCondition(expectedAliases == actualAliases)
                             .FailWith(Utils.DisplayEqualsFailure(Subject.SymbolType, "Aliases", expected, Subject.Aliases));

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public string DisplayString<T>(T input)
        {
            return input switch
            {
                null => "<null>",
                string s => $@"""{s}""",
                _ => input.ToString() ?? string.Empty
            };

        }
    }
}
