using FluentAssertions.Execution;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
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
                                  : Aliases.Prepend(Name).ToArray();
            using var scope = new AssertionScope();
            actual.Should().HaveName(Name)
                    .And.HaveDescription(Description)
                    .And.HaveAliases(expectedAliases)
                    .And.HaveIsHidden(IsHidden)
                    .And.HaveTreatUnmatchedTokensAsErrors(TreatUnmatchedTokensAsErrors);
        }
    }

    public class CommandOneSubCommandTestData : MakerCommandTestDataBase
    {
        // Option and ArgumentTestData of necessity test adding a single option and argument
        public CommandOneSubCommandTestData(string subCommandName)
              : base(new CommandDescriptor(null, null) { Name = DummyCommandName })
        {
            Descriptor.SubCommands.Add(
                new CommandDescriptor(null, null)
                {
                    Name = subCommandName
                });
            SubCommandName = subCommandName;
        }

        public string SubCommandName { get; }

        public override void Check(Command parentCommand)
        {
            var actual = parentCommand.Children.OfType<Command>().SingleOrDefault();

            using var scope = new AssertionScope();
            actual.Should().NotBeNull()
                .And.HaveName(SubCommandName);
        }
    }

}

