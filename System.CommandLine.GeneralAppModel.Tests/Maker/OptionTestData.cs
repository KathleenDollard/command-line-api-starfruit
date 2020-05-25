using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests;
using System.Linq;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class OptionBasicsTestData : MakerCommandTestDataBase
    {
        public OptionBasicsTestData(string name, string description, string[] aliases, bool isHidden, bool treatUnmatchedTokensAsErrors)
            : base(
                      new CommandDescriptor(null, null)
                      {
                          Name = name,
                          Description = description,
                          Aliases = aliases,
                          IsHidden = isHidden,
                          TreatUnmatchedTokensAsErrors = treatUnmatchedTokensAsErrors
                      }
                  )
        {
            Name = name;
            Description = description;
            Aliases = aliases;
            IsHidden = isHidden;
            TreatUnmatchedTokensAsErrors = treatUnmatchedTokensAsErrors;
        }

        public string Name { get; }
        public string Description { get; }
        public string[] Aliases { get; }
        public bool IsHidden { get; }
        public bool TreatUnmatchedTokensAsErrors { get; }

        public override void Check(Command actual)
        {
            var expectedAliases = Aliases is null
                                  ? new string[] { Name }
                                  : Aliases.Prepend(Name);
            using var scope = new AssertionScope();
            scope.ForCondition(actual.Name == Name)
                 .FailWith($"Expected Name to be {Utils.DisplayString(Name)}, but found {Utils.DisplayString(actual.Name)} ")
                 .Then
                 .ForCondition(actual.Description == Description)
                 .FailWith($"Expected Description to be {Utils.DisplayString(Description)}, but found {Utils.DisplayString(actual.Description)} ")
                 .Then
                 .ForCondition(Utils.CompareDistinctEnumerable(expectedAliases, actual.Aliases))
                 .FailWith($"Expected Aliases to be {Utils.DisplayString(expectedAliases)}, but found {Utils.DisplayString(actual.Aliases)} ")
                 .Then
                 .ForCondition(actual.IsHidden == IsHidden)
                 .FailWith($"Expected IsHidden to be {IsHidden}, but found {actual.IsHidden} ")
                 .Then
                 .ForCondition(actual.TreatUnmatchedTokensAsErrors == TreatUnmatchedTokensAsErrors)
                 .FailWith($"Expected TreatUnmatchedTokensAsErrors to be {TreatUnmatchedTokensAsErrors}, but found {actual.TreatUnmatchedTokensAsErrors} ")
                 .Then
                 .ForCondition(actual.Description == Description);
        }
    }

}
