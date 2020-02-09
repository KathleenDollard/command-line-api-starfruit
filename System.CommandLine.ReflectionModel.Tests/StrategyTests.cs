using FluentAssertions;
using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionAppModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xunit;

namespace System.CommandLine.ReflectionModel.Tests
{
    public class StrategyTests
    {

        private readonly ArgumentStrategies argumentStrategies = new ArgumentStrategies()
                            .HasStandardAttribute()
                            .HasStandardSuffixes();

        private readonly CommandStrategies commandStrategies = new CommandStrategies()
                            .HasStandardAttribute()
                            .HasStandardSuffixes();

        private readonly ArityStrategies arityStrategies = new ArityStrategies()
                            .HasStandardAttribute();

        private readonly DescriptionStrategies descriptionStrategies = new DescriptionStrategies()
                            .HasStandardAttribute();

        [Fact]
        public void AttributeStrategy_finds_argument_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("ArgumentSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = argumentStrategies.IsArgument(p);
            result.Should().BeTrue();
        }

        [Fact]
        public void Atribute_and_name_strategies_finds_no_argument_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("ArgumentSample").GetParameters().Where(p => p.Name == "C").Single();
            var result = argumentStrategies.IsArgument(p);
            result.Should().BeFalse();
        }

        [Fact]
        public void NameStrategy_finds_argument_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("ArgumentSample").GetParameters().Where(p => p.Name == "BArgument").Single();
            var result = argumentStrategies.IsArgument(p);
            result.Should().BeTrue();
        }

        [Fact]
        public void AttributeStrategy_finds_command_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("CommandSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = commandStrategies.IsCommand(p);
            result.Should().BeTrue();
        }

        [Fact]
        public void Atribute_and_name_strategies_finds_no_command_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("CommandSample").GetParameters().Where(p => p.Name == "C").Single();
            var result = commandStrategies.IsCommand(p);
            result.Should().BeFalse();
        }

        [Fact]
        public void NameStrategy_finds_Command_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("CommandSample").GetParameters().Where(p => p.Name == "BCommand").Single();
            var result = commandStrategies.IsCommand(p);
            result.Should().BeTrue();
        }

        [Fact]
        public void AttributeStrategy_finds_arity_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("AritySample").GetParameters().Where(p => p.Name == "A").Single();
            var result = arityStrategies.MinMax(p);
            result.Should().Be((1, 3));
        }

        [Fact]
        public void Atribute_strategy_finds_no_arity_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("AritySample").GetParameters().Where(p => p.Name == "C").Single();
            var result = arityStrategies.MinMax(p);
            result.Should().BeNull();
        }

        [Fact]
        public void AttributeStrategy_finds_description_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("DescriptionSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Fred");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("DescriptionSample").GetParameters().Where(p => p.Name == "C").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }

        #region "WIP"
        [Fact]
        public void AttributeStrategy_finds_description_on_argument_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("ArgumentSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Joe");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_argument_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("ArgumentSample").GetParameters().Where(p => p.Name == "D").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }

        [Fact]
        public void AttributeStrategy_finds_description_on_option_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("OptionSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Sue");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_option_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("OptionSample").GetParameters().Where(p => p.Name == "D").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }

        [Fact]
        public void AttributeStrategy_finds_description_on_command_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("CommandSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Sam");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_command_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("CommandSample").GetParameters().Where(p => p.Name == "D").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }
#endregion 
        [Fact]
        public void AttributeStrategy_finds_argument_on_property()
        {
            var p = typeof(ArgumentSampleClass).GetProperties().Where(p => p.Name == "A").Single();
            var result = argumentStrategies.IsArgument(p);
            result.Should().BeTrue();
        }

        [Fact]
        public void Atribute_and_name_strategies_finds_no_argument_on_property()
        {
            var p = typeof(ArgumentSampleClass).GetProperties().Where(p => p.Name == "C").Single();
            var result = argumentStrategies.IsArgument(p);
            result.Should().BeFalse();
        }

        [Fact]
        public void NameStrategy_finds_argument_on_property()
        {
            var p = typeof(ArgumentSampleClass).GetProperties().Where(p => p.Name == "BArgument").Single();
            var result = argumentStrategies.IsArgument(p);
            result.Should().BeTrue();
        }

        [Fact]
        public void AttributeStrategy_finds_command_on_property()
        {
            var p = typeof(CommandSampleClass).GetProperties().Where(p => p.Name == "A").Single();
            var result = commandStrategies.IsCommand(p);
            result.Should().BeTrue();
        }

        [Fact]
        public void Atribute_and_name_strategies_finds_no_command_on_property()
        {
            var p = typeof(CommandSampleClass).GetProperties().Where(p => p.Name == "C").Single();
            var result = commandStrategies.IsCommand(p);
            result.Should().BeFalse();
        }

        [Fact]
        public void NameStrategy_finds_Command_on_property()
        {
            var p = typeof(CommandSampleClass).GetProperties().Where(p => p.Name == "BCommand").Single();
            var result = commandStrategies.IsCommand(p);
            result.Should().BeTrue();
        }

        [Fact]
        public void AttributeStrategy_finds_arity_on_property()
        {
            var p = typeof(AritySampleClass).GetProperties().Where(p => p.Name == "A").Single();
            var result = arityStrategies.MinMax(p);
            result.Should().Be((1, 3));
        }

        [Fact]
        public void Atribute_strategy_finds_no_arity_on_property()
        {
            var p = typeof(AritySampleClass).GetProperties().Where(p => p.Name == "C").Single();
            var result = arityStrategies.MinMax(p);
            result.Should().BeNull();
        }

        [Fact]
        public void AttributeStrategy_finds_description_on_propertyn()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "A").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Fred");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "C").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }

        public void OptionSample([CmdOption(Description = "Sue")] string A, string BArgument, string C, [CmdOption()] string D) { }
        public void ArgumentSample([CmdArgument(Description="Joe")] string A, string BArgument, string C, [CmdArgument()] string D) { }
        public void CommandSample([CmdCommand(Description = "Sam")] string A, string BCommand, string C, [CmdCommand()] string D) { }
        public void AritySample([CmdArity(MinArgCount = 1, MaxArgCount = 3)] string A, string C) { }
        public void DescriptionSample([Description("Fred")] string A, string C) { }

        public class ArgumentSampleClass
        {
            [CmdArgument]
            public string A { get; set; }
            public string BArgument { get; set; }
            public string C { get; set; }
        }
        public class CommandSampleClass
        {
            [CmdCommand]
            public string A { get; set; }
            public string BCommand { get; set; }
            public string C { get; set; }
        }

        public class AritySampleClass
        {
            [CmdArity(MinArgCount = 1, MaxArgCount = 3)]
            public string A { get; set; }
            public string C { get; set; }
        }
        public class DescriptionSampleClass
        {
            [Description("Fred")]
            public string A { get; set; }
            public string C { get; set; }
        }
    }

}
