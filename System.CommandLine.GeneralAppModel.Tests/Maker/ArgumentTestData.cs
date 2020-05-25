using FluentAssertions.Execution;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class ArgumentBasicsTestData : MakerCommandTestDataBase
    {
        private const string DummyCommandName = "DummyCommandName";
        public ArgumentBasicsTestData(string name, string description, string[] aliases, bool isHidden, Type argumentType)
            : base(new CommandDescriptor(null, null) { Name = DummyCommandName })
        {
            base.Descriptor.Arguments.Add(
                new ArgumentDescriptor(null, null)
                {
                    Name = name,
                    Description = description,
                    Aliases = aliases,
                    IsHidden = isHidden,
                    ArgumentType = argumentType
                });
            Name = name;
            Description = description;
            Aliases = aliases;
            IsHidden = isHidden;
            ArgumentType = argumentType;
        }

        public string Name { get; }
        public string Description { get; }
        public string[] Aliases { get; }
        public bool IsHidden { get; }
        public Type ArgumentType { get; }

        public override void Check(Command command)
        {
            var actual = command.Arguments.FirstOrDefault();

            using var scope = new AssertionScope();
            actual.Should().HaveName(Name)
                       .And.HaveDescription(Description)
                       .And.HaveAliases(Aliases)
                       .And.HaveIsHidden(IsHidden)
                       .And.HaveArgumentType(ArgumentType);
        }
    }
}
