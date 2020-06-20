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
                      new CommandDescriptor(SymbolDescriptor.Empty, Utils.EmptyRawForTest)
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
            using var scope = new AssertionScope();

            actual.Should().NotBeNull();
            var expectedAliases = Aliases is null
                                  ? new string[] { Name }
                                  : Aliases.Prepend(Name).ToArray();
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
              : base(new CommandDescriptor(SymbolDescriptor.Empty, Utils.EmptyRawForTest) { Name = DummyCommandName })
        {
            Descriptor.SubCommands.Add(
                new CommandDescriptor(SymbolDescriptor.Empty, Utils.EmptyRawForTest)
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
              : base(new CommandDescriptor(SymbolDescriptor.Empty, Utils.EmptyRawForTest) { Name = DummyCommandName })
        {
            Descriptor.SubCommands.Add(
                new CommandDescriptor(SymbolDescriptor.Empty, Utils.EmptyRawForTest)
                {
                    Name = subCommandName1
                });
            Descriptor.SubCommands.Add(
                new CommandDescriptor(SymbolDescriptor.Empty, Utils.EmptyRawForTest)
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
              : base(new CommandDescriptor(SymbolDescriptor.Empty, typeof(CommandHandlerRunsTestData).GetMethod("Run")!)
              { Name = DummyCommandName })
        {
            Descriptor.Arguments.Add(
                    new ArgumentDescriptor(new ArgTypeInfo(typeof(string)), SymbolDescriptor.Empty, Utils.EmptyRawForTest)
                    {
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

    public class CommandInvokeMethodTestData : MakerCommandTestData
    {
        private static string checkValue = "";
        // Option and ArgumentTestData of necessity test adding a single option and argument
        public CommandInvokeMethodTestData()
              : base(new CommandDescriptor(SymbolDescriptor.Empty, Utils.EmptyRawForTest)
              {
                  Name = DummyCommandName,
                  InvokeMethod = new InvokeMethodInfo(typeof(CommandInvokeMethodTestData).GetMethod("Invoke")!, "Invoke", 0)
              })
        { }

        public int Invoke()
        {
            checkValue = $"Hello World";
            return 3;
        }

        public override void Check(Command command)
        {
            using var scope = new AssertionScope();
            var parser = new CommandLineBuilder(command)
                                 .UseDefaults()
                                 .Build();

            var parseResult = parser.Parse($"DummyCommandName");
            parseResult.Errors.Should().BeEmpty();

            var ret = parser.Invoke("");
            ret.Should().Be(3);
            checkValue.Should().Be("Hello World");
        }
    }

    public class CommandInvokeMethodMultipleParametersTestData : MakerCommandTestData
    {
        private const string HelloTo = "Universe";
        private const int RetValue = 5;
        private static string checkValue = "";
        // Option and ArgumentTestData of necessity test adding a single option and argument
        public CommandInvokeMethodMultipleParametersTestData()
              : base(new CommandDescriptor(SymbolDescriptor.Empty, Utils.EmptyRawForTest)
              {
                  Name = DummyCommandName,
                  InvokeMethod = new InvokeMethodInfo(typeof(CommandInvokeMethodMultipleParametersTestData).GetMethod("Invoke")!, "Invoke", 0)
              })
        {
            Descriptor.Arguments.Add(
                  new ArgumentDescriptor(new ArgTypeInfo(typeof(string)), SymbolDescriptor.Empty, Utils.EmptyRawForTest)
                  {
                      Name = "To"
                  }
              );
            Descriptor.Options.Add(
                 new OptionDescriptor( SymbolDescriptor.Empty, Utils.EmptyRawForTest)
                 {
                     Name = "AllCaps"
                 }
             );
        }

        public int Invoke(string to, bool allCaps)
        {
            checkValue = allCaps
                         ? $"Hello {to}".ToUpper()
                         : $"Hello {to}";
            return RetValue;
        }

        public override void Check(Command command)
        {
            using var scope = new AssertionScope();
            var parser = new CommandLineBuilder(command)
                                 .UseDefaults()
                                 .Build();

            string commandLine = $"{HelloTo} --all-caps";
            var parseResult = parser.Parse("DummyCommandName " + commandLine);
            parseResult.Errors.Should().BeEmpty();

            var ret = parser.Invoke(commandLine);
            ret.Should().Be(RetValue);
            checkValue.Should().Be($"HELLO UNIVERSE");
            // Values are ignored. If we do #69, this should be:
            //var ret = parser.Invoke($"{HelloTo}, {AllCaps}, {RetValue}" );
            //ret.Should().Be(RetValue);
            //checkValue.Should().Be($"Hello {HelloTo}");
        }
    }
}

