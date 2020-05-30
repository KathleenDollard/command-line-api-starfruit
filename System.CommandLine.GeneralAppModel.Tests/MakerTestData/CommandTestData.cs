using FluentAssertions;
using FluentAssertions.Execution;
using System.CommandLine.Builder;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class CommandBasicsTestData : MakerCommandTestData
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

    public class CommandOneSubCommandTestData : MakerCommandTestData
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

    public class CommandTwoSubCommandsTestData : MakerCommandTestData
    {
        // Option and ArgumentTestData of necessity test adding a single option and argument
        public CommandTwoSubCommandsTestData(string subCommandName1, string subCommandName2)
              : base(new CommandDescriptor(null, null) { Name = DummyCommandName })
        {
            Descriptor.SubCommands.Add(
                new CommandDescriptor(null, null)
                {
                    Name = subCommandName1
                });
            Descriptor.SubCommands.Add(
                new CommandDescriptor(null, null)
                {
                    Name = subCommandName2
                });
            SubCommandName1 = subCommandName1;
            SubCommandName2 = subCommandName2;
        }

        public string SubCommandName1 { get; }
        public string SubCommandName2 { get; }

        public override void Check(Command parentCommand)
        {
            using var scope = new AssertionScope();
            var actual1 = parentCommand.Children.OfType<Command>().FirstOrDefault();
            actual1.Should().NotBeNull()
                    .And.HaveName(SubCommandName1);

            var actual2 = parentCommand.Children.OfType<Command>().Skip(1).FirstOrDefault();
            actual2.Should().NotBeNull()
                    .And.HaveName(SubCommandName2);
        }
    }


    public class CommandHandlerRunsTestData : MakerCommandTestData
    {
        private const string HelloTo = "World";
        private static string checkValue = "";
        // Option and ArgumentTestData of necessity test adding a single option and argument
        public CommandHandlerRunsTestData()
              : base(new CommandDescriptor(null, typeof(CommandHandlerRunsTestData).GetMethod("Run"))
              { Name = DummyCommandName })
        {
            Descriptor.Arguments.Add(
                    new ArgumentDescriptor(null, null)
                    {
                        ArgumentType=typeof(string),
                        Name = "HelloTo"
                    }
                );
        }

        public int Run(string helloTo)
        {
            checkValue = $"Hello {helloTo}";
            return 3;
        }

        public override void Check(Command command)
        {
            using var scope = new AssertionScope();
            var parser = new CommandLineBuilder(command)
                                 .UseDefaults()
                                 .Build();

            var parseResult = parser.Parse($"DummyCommandName {HelloTo}");
            parseResult.Errors.Should().BeEmpty();

            var ret = parser.Invoke(HelloTo);
            ret.Should().Be(3);
            checkValue.Should().Be("Hello World");

        }
    }
}

