using FluentAssertions;
using System.CommandLine.ReflectionModel;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace System.CommandLine.StarFruit.Tests
{
    public class FunctionalSampleTests
    {
        private readonly ITestOutputHelper _output;

        public FunctionalSampleTests(ITestOutputHelper output)
            => _output = output;

        [Fact]
        public void Options_are_parsed_from_class_definition()
        {
            var nameValue = "Fred";
            var countValue = 3;
            var args = $"--name {nameValue} --uppercase --count {countValue}";
            BaseClass strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Name.Should().Be(nameValue);
            strongArg.Count.Should().Be(countValue);
            strongArg.Uppercase.Should().Be(true);
        }

        [Fact]
        public void Missing_option_do_not_cause_errors()
        {
            // Pretty much a smoke test
            var args = "";
            BaseClass strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Name.Should().Be(null);
            strongArg.Count.Should().Be(0);
            strongArg.Uppercase.Should().Be(false);
        }

        [Fact]
        public void Child_command_called_with_child_options()
        {
            var value = "hiya";
            var args = $"--greeting {value}";
            ChildClassOfEmpty strongArg = ReflectionParser<ChildClassOfEmpty>.GetInstance(args);
            strongArg.Greeting.Should().Be(value);
        }

        [Fact]
        public void Child_command_called()
        {
            var args = $"child-class";
            BaseClass strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Should().BeOfType(typeof(ChildClass));
        }

        [Fact]
        public void Child_command_called_with_parent_options()
        {
            var nameValue = "Fred";
            var countValue = 3;
            var args = $"--name {nameValue} --uppercase --count {countValue} child-class";
            BaseClass strongArg = ReflectionParser<BaseClass>.GetInstance(args);
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
            BaseClass strongArg = ReflectionParser<BaseClass>.GetInstance(args);
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

        [Fact(Skip ="Issue reported")]
        public void Bad_option_on_parent_gives_parser_error()
        {
            var countValue = "Joe";
            var args = $" --count {countValue} child-class";
            Assert.Throws<ArgumentException>(() => ReflectionParser<BaseClass>.GetInstance(args));
        }

        [Fact]
        public void Option_with_arity_two_accepts_two_arguments()
        {
            var args = $"Banana Waffles";
            Details strongArgs = ReflectionParser<Details>.GetInstance(args);
            strongArgs.FaveFoods.Should().BeEquivalentTo(new[] { "Banana", "Waffles" });
        }

        [Fact]
        public void Option_with_arity_two_or_three_accepts_three_arguments()
        {
            var args = $"Banana Waffles Chocolate --fave-year 2010";
            Details strongArgs = ReflectionParser<Details>.GetInstance(args);
            strongArgs.FaveFoods.Should().BeEquivalentTo(
                            new[] { "Banana", "Waffles", "Chocolate" });
        }

        [Fact]
        public void Option_with_arity_two_or_three_errors_with_four_args()
        {
            var args = $"Banana Waffles Chocolate Sushi --fave-year 2010";
            Assert.Throws<ArgumentException>(() => ReflectionParser<Details>
                                                        .GetInstance(args));
        }

        [Fact]
        public void Option_with_arity_two_or_three_errors_with_one_arguments()
        {
            var args = $"Banana";
            Assert.Throws<ArgumentException>(() => ReflectionParser<Details>.GetInstance(args));
        }

        [Fact]
        public void Option_with_default_uses_default()
        {
            var args = $"";
            Details strongArgs = ReflectionParser<Details>.GetInstance(args);
            strongArgs.FaveYear.Should().Be(2001);
        }

        [Fact(Skip = "Issue #769")]
        public void Option_with_range_gives_no_error_in_range()
        {
            var args = "--fave-year 2018";
            Details strongArgs = ReflectionParser<Details>.GetInstance(args);
            strongArgs.FaveYear.Should().Be(2018);
        }

        [Fact(Skip = "Issue #769")]
        public void Option_with_range_gives_error_if_below_range()
        {
            var args = "--fave-year 1918";
            Assert.Throws<ArgumentException>(() => ReflectionParser<Details>.GetInstance(args));
        }

        [Fact(Skip = "Issue #769")]
        public void Option_with_range_gives_error_if_above_range()
        {
            var args = "--fave-year 2028";
            Assert.Throws<ArgumentException>(() => ReflectionParser<Details>.GetInstance(args));
        }

        [Fact]
        public void Required_option_gives_error_if_missing()
        {
            var args = "--fave-year";
            Assert.Throws<ArgumentException>(() => ReflectionParser<Details2>.GetInstance(args));
        }

        [Fact]
        public void Required_option_gives_error_if_option_missing()
        {
            var args = "";
            Assert.Throws<ArgumentException>(() => ReflectionParser<Details2>.GetInstance(args));
        }

        [Fact]
        public void Option_details_from_attributes()
        {
            // How to test help?
        }

        [Fact]
        public void Arguments_are_parsed_from_class_definition()
        {
            var nameValue = "Fred";
            var birthdayValue = @"Happy Birthday";
            var countValue = 3;
            var args = $@"--name {nameValue} --uppercase --count {countValue} ""{birthdayValue}""";
            BaseClass strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Name.Should().Be(nameValue);
            strongArg.Count.Should().Be(countValue);
            strongArg.Uppercase.Should().Be(true);
            strongArg.BirthdayGreeting.Should().Be(birthdayValue);
        }

        [Fact]
        public void Missing_arguments_do_not_cause_errors_if_default()
        {
            // Pretty much a smoke test
            var args = "";
            BaseClass strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Name.Should().Be(null);
            strongArg.Count.Should().Be(0);
            strongArg.Uppercase.Should().Be(false);
        }

        [Fact]
        public void Child_command_called_with_child_arguments()
        {
            var value = @"See you later!";
            var args = @$"""{value}""";
            ChildClass strongArg = ReflectionParser<ChildClass>.GetInstance(args);
            strongArg.Farewell.Should().Be(value);
        }

        [Fact]
        public void Child_command_called_with_child_argument()
        {
            var nameValue = "Fred";
            var countValue = 3;
            var greetingValue = "hiya";
            var birthdayValue = @"Happy Birthday";
            var farewell = @"See you later!";
            var args = @$"--name {nameValue} --uppercase --count {countValue}  ""{birthdayValue}"" child-class --greeting {greetingValue} ""{farewell}""";
            BaseClass strongArg = ReflectionParser<BaseClass>.GetInstance(args);
            strongArg.Should().BeOfType(typeof(ChildClass));
            strongArg.Name.Should().Be(nameValue);
            strongArg.Count.Should().Be(countValue);
            strongArg.Uppercase.Should().Be(true);
            ((ChildClass)strongArg).BirthdayGreeting.Should().Be(birthdayValue);
            ((ChildClass)strongArg).Farewell.Should().Be(farewell);
            ((ChildClass)strongArg).Greeting.Should().Be(greetingValue);
        }




        public class BaseClass
        {
            [CmdArgument(DefaultValue = "Older")]
            public string BirthdayGreeting { get; set; }
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
            [CmdArgument(DefaultValue = "Yo")]
            public string Farewell { get; set; }
        }

        [CmdCommand(Description = "This is a command", Name = "Details")]
        public class Details
        {
            [CmdArgument(Description = "Your favorite foods", DefaultValue = "Pizza")]
            [CmdArity(MinArgCount = 2, MaxArgCount = 3)]
            public string[] FaveFoods { get; set; }

            [CmdOption(Description = "Your favorite year", DefaultValue = 2001)]
            [CmdRange(1950, 2020)]
            public int FaveYear { get; set; }
        }


        [CmdCommand(Description = "This is a command", Name = "Details")]
        public class Details2
        {
            [CmdOption(Description = "Your favorite year", OptionRequired = true, ArgumentRequired = true)]
            [CmdRange(1950, 2020)]
            public int FaveYear { get; set; }

        }
    }
}
