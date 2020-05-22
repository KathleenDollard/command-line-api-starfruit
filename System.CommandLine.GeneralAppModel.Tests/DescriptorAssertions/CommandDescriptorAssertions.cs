using FluentAssertions;
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
    public static class CommandDescriptorTestExtensions
    {
        public static CommandDescriptorAssertions Should(this CommandDescriptor instance)
        {
            return new CommandDescriptorAssertions(instance);
        }

        public static CommandDescriptorAssertions Should(this ObjectAssertions assertions)
        {
            return (CommandDescriptorAssertions)assertions.Subject;
        }
    }

    public class CommandDescriptorAssertions :
            ReferenceTypeAssertions<CommandDescriptor, CommandDescriptorAssertions>
    {
        public CommandDescriptorAssertions(CommandDescriptor instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "commanddescriptor";

        public AndConstraint<CommandDescriptorAssertions> BeEmptyExcept(params string[] skipChecks)
        {
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        //public (bool skip, AndConstraint<CommandDescriptorAssertions> constraint) Unless()
        //{
        //    return new AndConstraint<CommandDescriptorAssertions>(this);
        //}

        /// <summary>
        /// This determines if the object is in the initial state. "Empty" is a bit of a misnomer
        /// because Name and Raw should not be empty and are tested not being empty and ParentSymbolDescriptor is ignored
        /// </summary>
        /// <param name="because">Fluent feature, we're not generally using it</param>
        /// <param name="becauseArgs">Fluent feature, we're not generally using it</param>
        /// <returns></returns>
        public AndConstraint<CommandDescriptorAssertions> BeSameAs(CommandData commandData, string because = "", string becauseArgs = "")
        {
            using var _ = new AssertionScope();
            CheckCommand(commandData, Subject);

            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public void CheckSymbol<TSymbol, TDescriptor>(SymbolType symbolType, SymbolData expected, SymbolDescriptorBase actual)
            where TSymbol : SymbolData
            where TDescriptor : SymbolDescriptorBase
        {
            CheckType<TDescriptor>(actual);
            CheckIfNotNullWrapper(expected.AliasesWrapper, expectedValue => HaveAliases(symbolType, expectedValue, actual.Aliases));


            CheckName(symbolType, expected, actual);

            CheckValueOrDefault(expected.DescriptionWrapper, expectedValue => AreSame(symbolType, "Description", expectedValue, actual.Description));
            CheckValueOrDefault(expected.IsHiddenWrapper, expectedValue => AreSame(symbolType, "IsHidden", expectedValue, actual.IsHidden));
            CheckIfNotNullWrapper(expected.RawWrapper, expectedValue => AreSame(symbolType, "Raw type", expectedValue.GetType(), actual.Raw.GetType()));
            AreSame(symbolType, "SymbolType", symbolType, actual.SymbolType);
        }

        private void CheckName(SymbolType symbolType, SymbolData expected, SymbolDescriptorBase actual)
        {
    
            CheckIfNotNullWrapper(expected.NameWrapper, expectedValue =>
                {
                    if (actual.Name is null)
                    {
                        // If the parent symbol is null, then this is the root and all is well 
                        Execute.Assertion
                                        .ForCondition((actual.ParentSymbolDescriptorBase is null))
                                        .FailWith($@"Name cannot be null for {symbolType}");
                        return;
                    }
                    AreSame(symbolType, "Name", expectedValue.ToUpperInvariant(), actual.Name.ToUpperInvariant());
                });
        }


        private void CheckValueOrDefault<T>(Wrapper<T> wrapper, Action<T> check)
        {
            if (!(wrapper is null))
            {
                check(wrapper.Value);
            }
            else
            {
                check(default);
            }
        }

        private void CheckIfNotNullWrapper<T>(Wrapper<T> wrapper, Action<T> check)
        {
            if (!(wrapper is null))
            {
                check(wrapper.Value);
            }
        }

        private void CheckWithNullCheck<T>(Wrapper<T> wrapper, Action<T> check, Action checkIfWrapperNull)
        {
            if (!(wrapper is null))
            {
                check(wrapper.Value);
            }
            else
            {
                checkIfWrapperNull();
            }
        }


        public void CheckType<TExpected>(SymbolDescriptorBase actual)
        {
            Execute.Assertion
                 .ForCondition(typeof(TExpected).IsAssignableFrom(actual.GetType()))
                 .FailWith($"Expected Subject to be of type {typeof(TExpected).Name}, but found {actual.GetType().Name}");
            return;
        }

        public AndConstraint<CommandDescriptorAssertions> HaveAliases(SymbolType symbol, IEnumerable<string> expected, IEnumerable<string> actual)
        {
            Execute.Assertion
                 .ForCondition(actual.Count() == expected.Count())
                 .FailWith($"Expected Aliases for {symbol} to have count {expected.Count()}, but found {actual.Count()}")
                 .Then
                 .Given(() => ((string actual, string expected))(string.Join(", ", actual), string.Join(", ", expected)))
                 .ForCondition(t => t.actual == t.expected)
                 .FailWith("Expected Aliases for {symbol} to be {0}, but found {1}", t => t.expected, t => t.actual)
                 ;
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        //public AndConstraint<CommandDescriptorAssertions> HaveName(SymbolType symbol, string expected, string actual)
        //{
        //    Execute.Assertion
        //          .ForCondition(actual == expected)
        //          .FailWith($@"Expected Name for {symbol} to be {DisplayString(expected)}, but found {DisplayString(actual)}");
        //    return new AndConstraint<CommandDescriptorAssertions>(this);
        //}

        //public AndConstraint<CommandDescriptorAssertions> HaveDescription(SymbolType symbol, string description)
        //{
        //    Execute.Assertion
        //          .ForCondition(Subject.Description == description)
        //          .FailWith($@"Expected Description for {symbol} to be {DisplayString(description)}, but found {DisplayString(Subject.Description)}");
        //    return new AndConstraint<CommandDescriptorAssertions>(this);
        //}

        //public AndConstraint<CommandDescriptorAssertions> HaveIsHidden(SymbolType symbol, bool isHidden)
        //{
        //    Execute.Assertion
        //         .ForCondition(Subject.IsHidden == isHidden)
        //         .FailWith($@"Expected IsHidden for {symbol} to be {isHidden}, but found {Subject.IsHidden}");
        //    return new AndConstraint<CommandDescriptorAssertions>(this);
        //}

        //public AndConstraint<CommandDescriptorAssertions> HaveNonNullParent(SymbolType symbol)
        //{
        //    Execute.Assertion
        //         .ForCondition(!(Subject.ParentSymbolDescriptorBase is null))
        //         .FailWith($@"Expected ParentSymbolDescriptorBase for {symbol} to not be null");
        //    return new AndConstraint<CommandDescriptorAssertions>(this);
        //}

        //public AndConstraint<CommandDescriptorAssertions> HaveNullParent(SymbolType symbol)
        //{
        //    Execute.Assertion
        //         .ForCondition(Subject.ParentSymbolDescriptorBase is null)
        //         .FailWith($@"Expected ParentSymbolDescriptorBase for {symbol} to be null");
        //    return new AndConstraint<CommandDescriptorAssertions>(this);
        //}

        //public AndConstraint<CommandDescriptorAssertions> HaveRawOfType(SymbolType symbol, Type type)
        //{
        //    Execute.Assertion
        //         .ForCondition(type.IsAssignableFrom(Subject.Raw.GetType()))
        //         .FailWith($@"Expected Raw for {symbol} to be of type {type}, but found {Subject.Raw.GetType()}");
        //    return new AndConstraint<CommandDescriptorAssertions>(this);
        //}

        //public AndConstraint<CommandDescriptorAssertions> HaveSymbolType(SymbolType symbolType)
        //{
        //    Execute.Assertion
        //         .ForCondition(Subject.SymbolType == symbolType) // This should perhaps be a HasFlags
        //         .FailWith($@"Expected SymbolType to be of type {symbolType}, but found {Subject.SymbolType }");
        //    return new AndConstraint<CommandDescriptorAssertions>(this);
        //}

        //public AndConstraint<CommandDescriptorAssertions> HaveTreatUnmatchedTokensAsErrors(bool treatUnmatchedTokensAsErrors)
        //{
        //    Execute.Assertion
        //         .ForCondition(Subject.TreatUnmatchedTokensAsErrors == treatUnmatchedTokensAsErrors)
        //         .FailWith($@"Expected TreatUnmatchedTokensAsErrors to be {treatUnmatchedTokensAsErrors}, but found {Subject.TreatUnmatchedTokensAsErrors}");
        //    return new AndConstraint<CommandDescriptorAssertions>(this);
        //}

        public AndConstraint<CommandDescriptorAssertions> CheckOptions(IEnumerable<OptionData> expected, IEnumerable<OptionDescriptor> actual)
        {
            Execute.Assertion
                 .ForCondition(actual.Count() == expected.Count())
                 .FailWith($@"Expected Options to have count  {expected.Count()}, but found count {actual.Count()} items");
            var expectedArray = expected.ToArray();
            var actualArray = actual.ToArray();
            for (int i = 0; i < expectedArray.Length; i++)
            {
                CheckOption(expectedArray[i], actualArray[i]);
            }
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> CheckArguments(SymbolType symbol, IEnumerable<ArgumentData> expected, IEnumerable<ArgumentDescriptor> actual)
        {
            Execute.Assertion
                 .ForCondition(actual.Count() == expected.Count())
                 .FailWith($@"Expected Arguments for {symbol} to have count  {expected.Count()}, but found count {actual.Count()} items");
            var expectedArray = expected.ToArray();
            var actualArray = actual.ToArray();
            for (int i = 0; i < expectedArray.Length; i++)
            {
                CheckArgument(symbol, expectedArray[i], actualArray[i]);
            }
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> CheckSubCommands(string parentName, IEnumerable<CommandData> expected, IEnumerable<CommandDescriptor> actual)
        {
            Execute.Assertion
                 .ForCondition(actual.Count() == expected.Count())
                 .FailWith($@"Expected SubCommands for {parentName} to have count  {expected.Count()}, but found count {actual.Count()} items");
            var expectedArray = expected.ToArray();
            var actualArray = actual.ToArray();
            for (int i = 0; i < expectedArray.Length; i++)
            {
                CheckCommand(expectedArray[i], actualArray[i]);
            }
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        private void CheckArgument(SymbolType symbol, ArgumentData expected, ArgumentDescriptor actual)
        {
            CheckSymbol<ArgumentData, ArgumentDescriptor>(SymbolType.Argument, expected, actual);
            CheckValueOrDefault(expected.RequiredWrapper, value => AreSame(SymbolType.Argument, "Required", value, actual.Required));
            CheckIfNotNullWrapper(expected.ArgumentTypeWrapper, value => AreSame(SymbolType.Argument, "ArgumentType", value, actual.ArgumentType));
            CheckArity(SymbolType.Argument, expected, actual.Arity);
            CheckDefault(SymbolType.Argument, expected, actual.DefaultValue);
        }

        private void CheckCommand(CommandData expected, CommandDescriptor actual)
        {
            CheckSymbol<CommandData, CommandDescriptor>(SymbolType.Command, expected, actual);
            CheckValueOrDefault(expected.TreatUnmatchedTokensAsErrorsWrapper, expectedValue => AreSame(SymbolType.Command, "TreatUnmatchedTokensAsErrors", expectedValue, actual.TreatUnmatchedTokensAsErrors));
            CheckIfNotNullWrapper(expected.OptionsWrapper, expectedValue => CheckOptions(expectedValue, actual.Options));
            CheckIfNotNullWrapper(expected.SubCommandsWrapper, expectedValue => CheckSubCommands(actual.Name, expectedValue, actual.SubCommands));
            CheckIfNotNullWrapper(expected.ArgumentsWrapper, expectedValue => CheckArguments(SymbolType.Command, expectedValue, actual.Arguments));
        }

        private void CheckOption(OptionData expected, OptionDescriptor actual)
        {
            CheckSymbol<OptionData, OptionDescriptor>(SymbolType.Option, expected, actual);
            CheckValueOrDefault(expected.RequiredWrapper, value => AreSame(SymbolType.Option, "Required", value, actual.Required));
        }

        public AndConstraint<CommandDescriptorAssertions> AreSame<T>(SymbolType symbol, string valueName, T expected, T actual)
        {
            Execute.Assertion
                 .ForCondition(Equals(expected, actual))
                 .FailWith($@"Expected {valueName} for {symbol} to be {DisplayString(expected)}, but found {DisplayString(actual)}");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> CheckArity(SymbolType symbol, ArgumentData expectedArgument, ArityDescriptor actual)
        {
            var expectArity = expectedArgument.HasArityWrapper is null
                                ? false
                                : expectedArgument.HasArityWrapper.Value;
            Execute.Assertion
                .ForCondition(expectArity != (actual is null))
                .FailWith(expectArity
                            ? $@"Expected an Arity for {symbol}, but one was not found"
                            : $@"No Arity was expected for {symbol}, but one was found");
            if (expectArity)
            {
                var min = expectedArgument.ArityMinWrapper is null
                                ? 0
                                : expectedArgument.ArityMinWrapper.Value;
                var max = expectedArgument.ArityMaxWrapper is null
                       ? 0
                       : expectedArgument.ArityMaxWrapper.Value;
                Execute.Assertion
                    .ForCondition(actual.MinimumNumberOfValues == min && actual.MaximumNumberOfValues == max)
                    .FailWith($@"Expected Arity for {symbol} to be {min} to {max}, but found {actual.MinimumNumberOfValues} to {actual.MaximumNumberOfValues}");
            }
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> CheckDefault(SymbolType symbol, ArgumentData expectedArgument, DefaultValueDescriptor actual)
        {
            var expectDefault = expectedArgument.HasDefaultWrapper is null
                                ? false
                                : expectedArgument.HasDefaultWrapper.Value;
            Execute.Assertion
                .ForCondition(expectDefault != (actual is null))
                .FailWith(expectDefault
                            ? $@"Expected a Default for {symbol}, but one was not found"
                            : $@"No Default was expected for {symbol}, but one was found");
            if (expectDefault)
            {
                var defaultValue = expectedArgument.DefaultValueWrapper is null
                                ? 0
                                : expectedArgument.DefaultValueWrapper.Value;
                Execute.Assertion
                    .ForCondition(actual.DefaultValue.Equals(defaultValue))
                    .FailWith($@"Expected DefaultValue for {symbol} to be {defaultValue}, but found {actual.DefaultValue}");
            }
            return new AndConstraint<CommandDescriptorAssertions>(this);
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

