using FluentAssertions;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace System.CommandLine.StarFruit.Tests
{
    public class ConfigureFromClassTests
    {
        private readonly ITestOutputHelper _output;

        public ConfigureFromClassTests(ITestOutputHelper output)
            => _output = output;

        [Fact]
        public void Options_are_parsed_from_class_definition()
        {
            var nameValue = "Fred";
            var countValue = 3;
            var args = $"--name {nameValue} --uppercase --count {countValue}";
            var strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Name.Should().Be(nameValue);
            strongArg.Count.Should().Be(countValue);
            strongArg.Uppercase.Should().Be(true);
        }

        [Fact]
        public void Options_are_not_parsed_from_class_definition_when_missing()
        {
            var args = "";
            var strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Name.Should().Be(null);
            strongArg.Count.Should().Be(0);
            strongArg.Uppercase.Should().Be(false);
        }

        [Fact]
        public void Child_command_called_with_child_options()
        {
            var value = "hiya";
            var args = $"--greeting {value}";
            var strongArg = ReflectionParser<ChildClassOfEmpty>.GetInstance(args);
            strongArg.Greeting.Should().Be(value);
        }

        [Fact]
        public void Child_command_called()
        {
            var args = $"child-class";
            var strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Should().BeOfType(typeof(ChildClass));
        }

        [Fact]
        public void Child_command_called_with_parent_options()
        {
            var nameValue = "Fred";
            var countValue = 3;
            var args = $"--name {nameValue} --uppercase --count {countValue} child-class";
            var strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Should().BeOfType(typeof(ChildClass));
            strongArg.Name.Should().Be(nameValue);
            strongArg.Count.Should().Be(countValue);
            strongArg.Uppercase.Should().Be(true);
        }

        [Fact]
        public void Child_command_called_with_parent_and_child_options()
        {
            var nameValue = "Fred";
            var countValue = 3;
            var greetingValue = "hiya";
            var args = $"--name {nameValue} --uppercase --count {countValue} child-class --greeting {greetingValue}";
            var strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Should().BeOfType(typeof(ChildClass));
            strongArg.Name.Should().Be(nameValue);
            strongArg.Count.Should().Be(countValue);
            strongArg.Uppercase.Should().Be(true);
            ((ChildClass)strongArg).Greeting.Should().Be(greetingValue);
        }

        [Fact]
        public void Bad_option_type_gives_parser_error()
        {
            var countValue = "Joe";
            var args = $"--count {countValue}";
            Assert.Throws<ArgumentException>(() => ReflectionParser<BaseClass>.GetInstance(args));
        }

        [Fact]
        public void Bad_option_on_parent_gives_parser_error()
        {
            var countValue = "Joe";
            var args = $"--count {countValue} child-class";
            Assert.Throws<ArgumentException>(() => ReflectionParser<BaseClass>.GetInstance(args));
        }

        [Fact]
        public void Option_with_arity_two_accepts_two_arguments()
        {
            var args = $"--fave-foods Banana Waffles";
            var strongArgs = ReflectionParser<OptionDetails>.GetInstance(args);
            strongArgs.FaveFoods.Should().BeEquivalentTo(new [] { "Banana", "Waffles"});
        }

        [Fact]
        public void Option_with_arity_two_errors_with_three_arguments()
        {
            var args = $"--fave-foods Banana Waffles Chocolate --fave-year 2010";
            Assert.Throws<ArgumentException>(() => ReflectionParser<OptionDetails>
                                                        .GetReflectionParser()
                                                        .GetInstance(args));
        }

        [Fact]
        public void Option_with_arity_two_errors_with_one_arguments()
        {
            var args = $"--fave-foods Banana";
            Assert.Throws<ArgumentException>(() => ReflectionParser<OptionDetails>.GetInstance(args));
        }

        [Fact]
        public void Option_with_default_uses_default()
        {
            var args = $"";
            var strongArgs = ReflectionParser<OptionDetails>.GetInstance(args);
            strongArgs.FaveYear.Should().Be(2001);
        }

        [Fact]
        public void Option_with_range_gives_no_error_in_range()
        {
            var args = "--fave-year 2018";
            var strongArgs = ReflectionParser<OptionDetails>.GetInstance(args);
            strongArgs.FaveYear.Should().Be(2018);
        }

        [Fact]
        public void Option_with_range_gives_error_if_below_range()
        {
            var args = "--fave-year 1918";
            Assert.Throws<ArgumentException>(() => ReflectionParser<OptionDetails>.GetInstance(args));
        }

        [Fact]
        public void Option_with_range_gives_error_if_above_range()
        {
            var args = "--fave-year 2028";
            Assert.Throws<ArgumentException>(() => ReflectionParser<OptionDetails>.GetInstance(args));
        }

        public class BaseClass
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public bool Uppercase { get; set; }
        }

        public class EmptyBaseClass
        { }

        public class ChildClassOfEmpty : EmptyBaseClass
        {
            public string Greeting { get; set; }
        }

        public class ChildClass : BaseClass
        {
            public string Greeting { get; set; }
        }

        public class OptionDetails
        {
            [CmdArgCount(2,3)]
            public string[] FaveFoods { get; set; }

            [CmdDefaultValue(2001)]
            [CmdRange(1950, 2020)]
            public int FaveYear { get; set; }

        }
    }
}
