using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class SymbolDescriptorAssertions<TDesc, TAssert> :
            ReferenceTypeAssertions<TDesc, TAssert>
        where TDesc : SymbolDescriptorBase
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
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Name", expected, Subject.Name));

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public AndConstraint<TAssert> HaveDescription(string expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.Description == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Description", expected, Subject.Description));

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public AndConstraint<TAssert> HaveIsHidden(bool expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.IsHidden == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "IsHidden", expected, Subject.IsHidden));

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public AndConstraint<TAssert> HaveAliases(string[] expected)
        {
            Execute.Assertion
                     .ForCondition(Subject.Aliases == expected)
                     .FailWith(Utils.DisplayEqualsFailure(SymbolType.Command, "Aliases", expected, Subject.Aliases));

            return new AndConstraint<TAssert>((TAssert)this);
        }

        public string DisplayString<T>(T input)
        {
            return input switch
            {
                null => "<null>",
                string s => $@"""{s}""",
                _ => input.ToString()
            };

        }
    }
}
