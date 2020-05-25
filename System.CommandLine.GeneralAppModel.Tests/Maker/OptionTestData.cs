using FluentAssertions.Execution;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Parsing;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class OptionBasicsTestData : MakerCommandTestDataBase
    {
        private const string DummyCommandName = "DummyCommandName";
        public OptionBasicsTestData(string name, string description, string[] aliases, bool isHidden, bool required)
              : base(new CommandDescriptor(null, null) { Name = DummyCommandName })
        {
            base.Descriptor.Options.Add(
                        new OptionDescriptor(null, null)
                        {
                            Name = name,
                            Description = description,
                            Aliases = aliases,
                            IsHidden = isHidden,
                            Required = required
                        }
                    );
            Name = name;
            Description = description;
            Aliases = aliases;
            IsHidden = isHidden;
            Required = required;
        }

        public string Name { get; }
        public string Description { get; }
        public string[] Aliases { get; }
        public bool IsHidden { get; }
        public bool Required { get; }

        public override void Check(Command  command)
        {
            var actual = command.Options.FirstOrDefault();
            var name = Name.ToKebabCase().ToLowerInvariant();

            var expectedAliases = Aliases is null
                                  ? new string[] { name }
                                  : Aliases.Prepend(name).ToArray();
            using var scope = new AssertionScope();
            actual.Should().NotBeNull()
                       .And.HaveName(name)
                       .And.HaveDescription(Description)
                       .And.HaveAliases(expectedAliases)
                       .And.HaveIsHidden(IsHidden)
                       .And.HaveRequired(Required);
        }
    }

}
