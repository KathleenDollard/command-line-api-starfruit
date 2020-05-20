using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
        public AndConstraint<CommandDescriptorAssertions> BeEmpty(string because = "", string becauseArgs = "")
        {
            using var _ = new AssertionScope();
            Subject.Should().HaveType<CommandDescriptor>();
            Subject.Should().HaveAliases();
            Subject.Should().HaveDescription(null);
            Subject.Should().HaveIsHidden(false);
            Subject.Should().HaveRawOfType(typeof(object));
            Subject.Should().HaveSymbolType(SymbolType.Command);
            Subject.Should().HaveTreatUnmatchedTokensAsErrors(false);
            Subject.Should().HaveEmptyOptions();
            Subject.Should().HaveEmptySubCommands();
            Subject.Should().HaveEmptyArguments()


            //var x =   Execute.Assertion
            //       .BecauseOf(because, becauseArgs)
            //.Should().BeOfType<AssertionScope >().And.Should()  // The need of this implies something isn't quite right here
            //     .HaveType<CommandDescriptor>()
            //.HaveAliases().And
            //.HaveDescription("").And
            //.HaveIsHidden(false).And
            //.HaveNonNullParent().And
            //.HaveRawOfType(typeof(object)).And
            //.HaveSymbolType(SymbolType.Command).And
            //.HaveTreatUnmatchedTokensAsErrors(false).And
            //.HaveEmptyOptions().And
            //.HaveEmptySubCommands().And
            //.HaveEmptyArguments()
            ;

            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveType<T>()
        {
            Execute.Assertion
                 .ForCondition(typeof(T).IsAssignableFrom(Subject.GetType()))
                 .FailWith($"Expected Subject to be of type {typeof(T).Name}, but found {Subject.GetType().Name}");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }


        public AndConstraint<CommandDescriptorAssertions> HaveAliases(params string[] aliases)
        {
            Execute.Assertion
                 .ForCondition(Subject.Aliases.Count() == aliases.Count())
                 .FailWith($"Expected Aliases to have count {aliases.Count()}, but found {Subject.Aliases.Count()}")
                 .Then
                 .Given(() => ((string actual, string expected))(String.Join(", ", Subject.Aliases), String.Join("|", aliases)))
                 .ForCondition(t => t.actual == t.expected)
                 .FailWith($"Expected Aliases to have count {0}, but found {1}", t => t.expected, t => t.actual)
                 ;
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveDescription(string description)
        {
            Execute.Assertion
                  .ForCondition(Subject.Description == description)
                  .FailWith($@"Expected Description to be {DisplayString(description)}, but found {DisplayString(Subject.Description)}");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveName(string name)
        {
            Execute.Assertion
                .ForCondition(Subject.Name == name)
                .FailWith($@"Expected Name to be {DisplayString(name)}, but found {DisplayString(Subject.Name)}");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveIsHidden(bool isHidden)
        {
            Execute.Assertion
                 .ForCondition(Subject.IsHidden == isHidden)
                 .FailWith($@"Expected IsHidden to be {isHidden}, but found {Subject.IsHidden}");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveNonNullParent()
        {
            Execute.Assertion
                 .ForCondition(!(Subject.ParentSymbolDescriptorBase is null))
                 .FailWith($@"Expected ParentSymbolDescriptorBase to not be null");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveNullParent()
        {
            Execute.Assertion
                 .ForCondition(Subject.ParentSymbolDescriptorBase is null)
                 .FailWith($@"Expected ParentSymbolDescriptorBase to be null");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveRawOfType(Type type)
        {
            Execute.Assertion
                 .ForCondition(type.IsAssignableFrom(Subject.Raw.GetType()))
                 .FailWith($@"Expected Raw to be of type {type}, but found {Subject.Raw.GetType()}");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveSymbolType(SymbolType symbolType)
        {
            Execute.Assertion
                 .ForCondition(Subject.SymbolType == symbolType) // This should perhaps be a HasFlags
                 .FailWith($@"Expected SymbolType to be of type {symbolType}, but found {Subject.SymbolType }");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveTreatUnmatchedTokensAsErrors(bool treatUnmatchedTokensAsErrors)
        {
            Execute.Assertion
                 .ForCondition(Subject.TreatUnmatchedTokensAsErrors == treatUnmatchedTokensAsErrors)
                 .FailWith($@"Expected TreatUnmatchedTokensAsErrors to be {treatUnmatchedTokensAsErrors}, but found {Subject.TreatUnmatchedTokensAsErrors}");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveEmptyOptions()
        {
            Execute.Assertion
                 .ForCondition(!Subject.Options.Any())
                 .FailWith($@"Expected Options to be empty, but found {Subject.Options.Count()} items");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveEmptyArguments()
        {
            Execute.Assertion
                 .ForCondition(!Subject.Arguments.Any())
                 .FailWith($@"Expected Arguments to be empty, but found {Subject.Arguments.Count()} items");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public AndConstraint<CommandDescriptorAssertions> HaveEmptySubCommands()
        {
            Execute.Assertion
                 .ForCondition(!Subject.SubCommands.Any())
                 .FailWith($@"Expected SubCommands to be empty, but found {Subject.SubCommands.Count()} items");
            return new AndConstraint<CommandDescriptorAssertions>(this);
        }

        public string DisplayString(string input)
        {
            return input is null
                    ? "<null>"
                    : $@"""{input}""";
        }
    }
}

