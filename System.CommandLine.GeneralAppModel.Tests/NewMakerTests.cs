using FluentAssertions;
using FluentAssertions.Equivalency;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests;
using System.CommandLine.GeneralAppModel.Tests.Maker;
using System.Linq;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class NewMakerTests
    {
        public const string name = "Fred";
        public const string nameForEmpty = "NameIsRequired";
        public const string desc = "This is awesome!";
        public const string aliasAsStringMuitple = "x,y,z";
        public const string aliasAsStringSingle = "a";

        [Theory]
        [InlineData(name, desc, aliasAsStringMuitple, true, true)]
        [InlineData(name, desc, aliasAsStringSingle, false, false)]
        [InlineData(nameForEmpty, null, null, false, false)]
        public void CommandBasicsTests(string name, string description, string aliasesAsString, bool isHidden, bool treatUnmatchedTokensAsErrors)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();
            var data = new CommandBasicsTestData(name, description, aliases, isHidden, treatUnmatchedTokensAsErrors);
            var command = CommandMaker.MakeCommand(data.Descriptor);
            data.Check(command);
        }
    }
}
