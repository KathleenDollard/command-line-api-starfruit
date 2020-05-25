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

        [Theory]
        [InlineData(name, desc, aliasAsStringMuitple, true, true)]
        [InlineData(name, desc, aliasAsStringSingle, false, false)]
        [InlineData(nameForEmpty, null, null, false, false)]
        public void OptionBasicsTests(string name, string description, string aliasesAsString, bool isHidden, bool required)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();
            var data = new OptionBasicsTestData(name, description, aliases, isHidden, required);
            var command = CommandMaker.MakeCommand(data.Descriptor);
            data.Check(command);
        }

        [Theory]
        [InlineData(name, desc, aliasAsStringMuitple, true, typeof(string))]
        [InlineData(name, desc, aliasAsStringSingle, false, typeof(string))]
        [InlineData(nameForEmpty, null, null, false, null)]
        public void ArgumentBasicsTests(string name, string description, string aliasesAsString, bool isHidden, Type argumentType)
        {
            var aliases = aliasesAsString is null
                          ? null
                          : aliasesAsString.Split(",").Select(s => s.Trim()).ToArray();
            var data = new ArgumentBasicsTestData(name, description, aliases, isHidden, argumentType);
            var command = CommandMaker.MakeCommand(data.Descriptor);
            data.Check(command);
        }

        [Theory]
        [InlineData(true, 0, 5)]
        [InlineData(false, null, null)]
        [InlineData(true, 2, 2)]
        public void ArgumentArityTests(bool isSet, int? minValue, int? maxValue)
        {
            var data = new ArgumentArityTestData(isSet, minValue, maxValue);
            var command = CommandMaker.MakeCommand(data.Descriptor);
            data.Check(command);
        }
    }
}
