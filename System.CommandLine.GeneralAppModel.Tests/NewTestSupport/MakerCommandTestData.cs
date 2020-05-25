using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests;
using System.Linq;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class CommandBasicsTestData : MakerCommandTestDataBase
    {
        public CommandBasicsTestData(string name, string description, string[] aliases, bool isHidden, bool treatUnmatchedTokensAsErrors)
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


    //public class CommandAllValuesTestData : MakerCommandTestDataBase
    //{
    //    public const string name = "Fred";
    //    public const string desc = "This is awesome!";
    //    public static readonly string[] aliases = new string[] { "x", "y", "z" };
    //    public CommandBasicsTestData()
    //        : base(
    //                  new CommandDescriptor(null, null)
    //                  {
    //                      Name = name,
    //                      Description = desc,
    //                      Aliases = aliases,
    //                      IsHidden = true,
    //                      TreatUnmatchedTokensAsErrors = true
    //                  }
    //              )
    //    { }

    //    public override void Check(Command actual)
    //    {
    //        using var _ = new AssertionScope();
    //        actual.Name.Should().Be(name);
    //        actual.Description.Should().Be(desc);
    //        actual.Aliases.Should().BeSameAs(aliases.Prepend(name));
    //        actual.IsHidden.Should().BeTrue();
    //        actual.TreatUnmatchedTokensAsErrors.Should().BeTrue();
    //    }
    //}

    //public class CommandEmptyTestData : MakerCommandTestDataBase
    //{
    //    public const string name = "NameIsRequired";
    //    public CommandEmptyTestData()
    //        : base(
    //                  new CommandDescriptor(null, null)
    //                  {
    //                      Name = name
    //                  }
    //              )
    //    { }

    //    public override void Check(Command actual)
    //    {
    //        using var scope = new AssertionScope();
    //        scope.ForCondition(actual.Name == name)
    //             .FailWith($"Expected Name to be {actual.Name}, but found {name} ")
    //             .Then
    //             .ForCondition(actual.Description == descrip)
    //        actual.Name.Should().Be(name);
    //        actual.Description.Should().Be(null);
    //        actual.Aliases.Should().BeSameAs(null);
    //        actual.IsHidden.Should().BeFalse();
    //        actual.TreatUnmatchedTokensAsErrors.Should().BeFalse();
    //    }
    //}

 
    public class OptionBasicsTestData : MakerOptionTestDataBase
    {
        public const string name = "George";
        public const string desc = "This is awesome option!";
        public static readonly string[] aliases = new string[] { "x", "y", "zed" };
        public OptionBasicsTestData()
            : base(
                      new OptionDescriptor(null, null)
                      {
                          Name = name,
                          Description = desc,
                          Aliases = aliases,
                          IsHidden = true,
                          Required = true
                      }
                  )
        { }

        public override void Check(Option actual)
        {
            using var _ = new AssertionScope();
            actual.Name.Should().Be(name);
            actual.Description.Should().Be(desc);
            actual.Aliases.Should().BeSameAs(aliases);
            actual.IsHidden.Should().BeTrue();
            actual.Required.Should().BeTrue();
        }
    }

    public class OptionEmptyTestData : MakerOptionTestDataBase
    {
        public const string name = "NameIsRequired";
        public OptionEmptyTestData()
            : base(
                      new OptionDescriptor(null, null)
                      {
                          Name = name,
                      }
                  )
        { }

        public override void Check(Option actual)
        {
            using var _ = new AssertionScope();
            actual.Name.Should().Be(name);
            actual.Description.Should().Be(null);
            actual.Aliases.Should().BeSameAs(null);
            actual.IsHidden.Should().BeFalse();
            actual.Required.Should().BeFalse();
        }
    }

    public class ArgumentBasicsTestData : MakerArgumentTestDataBase
    {
        public const string name = "Fred";
        public const string desc = "This is awesome!";
        public static readonly string[] aliases = new string[] { "x", "y", "z" };
        public static readonly Type argType = typeof(string);
        public ArgumentBasicsTestData()
            : base(
                      new ArgumentDescriptor(null, null)
                      {
                          Name = name,
                          Description = desc,
                          Aliases = aliases,
                          IsHidden = true,
                          Required = true,
                          ArgumentType = argType
                      }
                  )
        { }

        public override void Check(Argument actual)
        {
            using var _ = new AssertionScope();
            actual.Name.Should().Be(name);
            actual.Description.Should().Be(desc);
            actual.Aliases.Should().BeSameAs(aliases);
            actual.IsHidden.Should().BeTrue();
            actual.ArgumentType.Should().Be (argType );
            // Jon: Check whether required is intended for arguments
            //  actual.Required.Should().BeTrue();
        }
    }

    public class ArgumentEmptyTestData : MakerArgumentTestDataBase
    {
        public const string name = "NameIsRequired";
        public ArgumentEmptyTestData()
            : base(
                      new ArgumentDescriptor(null, null)
                      {
                          Name = name
                      }
                  )
        { }

        public override void Check(Argument actual)
        {
            using var _ = new AssertionScope();
            actual.Name.Should().Be(name);
            actual.Description.Should().Be(null);
            actual.Aliases.Should().BeSameAs(null);
            actual.IsHidden.Should().BeFalse();
            actual.ArgumentType.Should().Be(typeof(int));
            // actual.Required.Should().BeFalse();
        }
    }
}
