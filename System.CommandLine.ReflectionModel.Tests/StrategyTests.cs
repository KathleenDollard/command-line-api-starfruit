using FluentAssertions;
using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xunit;

namespace System.CommandLine.ReflectionModel.Tests
{
    public class StrategyTests
    {

        private readonly ArgumentStrategies argumentStrategies = new ArgumentStrategies()
                            .AllStandard();

        private readonly CommandStrategies commandStrategies = new CommandStrategies()
                            .AllStandard();

        private readonly ArityStrategies arityStrategies = new ArityStrategies()
                            .AllStandard();

        private readonly DescriptionStrategies descriptionStrategies = new DescriptionStrategies()
                            .AllStandard();

        private readonly NameStrategies nameStrategies = new NameStrategies()
                            .AllStandard();

        private readonly IsRequiredStrategies requiredStrategies = new IsRequiredStrategies()
                         .AllStandard();

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

        #region "New"

        [Fact]
        public void AttributeStrategy_finds_name_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("NameSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("Terry");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("NameSample").GetParameters().Where(p => p.Name == "C").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("C");
        }

        [Fact]
        public void AttributeStrategy_finds_name_on_argument_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("ArgumentSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("A2");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_argument_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("ArgumentSample").GetParameters().Where(p => p.Name == "D").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("D");
        }

        [Fact]
        public void AttributeStrategy_finds_name_on_option_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("OptionSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("A2");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_option_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("OptionSample").GetParameters().Where(p => p.Name == "D").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("D");
        }

        [Fact]
        public void AttributeStrategy_finds_name_on_command_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("CommandSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("A2");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_command_attribute_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("CommandSample").GetParameters().Where(p => p.Name == "D").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("D");
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
        public void AttributeStrategy_finds_description_on_property()
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

        [Fact]
        public void AttributeStrategy_finds_description_on_argument_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "D").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Mary");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_argument_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "E").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }

        [Fact]
        public void AttributeStrategy_finds_description_on_option_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "F").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Jane");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_option_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "G").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }

        [Fact]
        public void AttributeStrategy_finds_description_on_command_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "H").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Jill");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_command_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "I").Single();
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }


        [Fact]
        public void AttributeStrategy_finds_name_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "A").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("Terry");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "C").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("C");
        }

        [Fact]
        public void AttributeStrategy_finds_name_on_argument_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "D").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("D2");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_argument_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "E").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("E");
        }

        [Fact]
        public void AttributeStrategy_finds_name_on_option_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "F").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("F2");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_option_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "G").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("G");
        }

        [Fact]
        public void AttributeStrategy_finds_name_on_command_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "H").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("H2");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_command_attribute_on_property()
        {
            var p = typeof(DescriptionSampleClass).GetProperties().Where(p => p.Name == "I").Single();
            var result = nameStrategies.Name(p);
            result.Should().Be("I");
        }

        [Fact]
        public void AttributeStrategy_finds_name_on_type()
        {
            var p = typeof(DescriptionSampleClass);
            var result = nameStrategies.Name(p);
            result.Should().Be("Charlie");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_type()
        {
            var p = typeof(DescriptionSampleClass3);
            var result = nameStrategies.Name(p);
            result.Should().Be("DescriptionSampleClass3");
        }


        [Fact]
        public void AttributeStrategy_finds_description_on_type()
        {
            var p = typeof(DescriptionSampleClass);
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Frank");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_type()
        {
            var p = typeof(DescriptionSampleClass3);
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }

        [Fact]
        public void AttributeStrategy_finds_name_on_command_attribute_on_type()
        {
            var p = typeof(DescriptionSampleClass2);
            var result = nameStrategies.Name(p);
            result.Should().Be("Jean");
        }

        [Fact]
        public void Atribute_strategy_uses_default_when_no_name_on_command_attribute_on_type()
        {
            var p = typeof(DescriptionSampleClass3);
            var result = nameStrategies.Name(p);
            result.Should().Be("DescriptionSampleClass3");
        }

        [Fact]
        public void AttributeStrategy_finds_description_on_command_attribute_on_type()
        {
            var p = typeof(DescriptionSampleClass2);
            var result = descriptionStrategies.Description(p);
            result.Should().Be("Will");
        }

        [Fact]
        public void Atribute_strategy_finds_no_description_on_command_attribute_on_type()
        {
            var p = typeof(DescriptionSampleClass3);
            var result = descriptionStrategies.Description(p);
            result.Should().BeNull();
        }

        [Fact]
        public void Attribute_strategy_finds_OptionRequired_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("OptionSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = requiredStrategies.IsRequired(p, SymbolType.Option);
            result.Should().BeTrue();
        }

        [Fact]
        public void Attribute_strategy_finds_no_OptionRequired_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("OptionSample").GetParameters().Where(p => p.Name == "D").Single();
            var result = requiredStrategies.IsRequired(p, SymbolType.Option);
            result.Should().BeFalse();
        }

        [Fact]
        public void Attribute_strategy_finds_ArgumentRequired_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("OptionSample").GetParameters().Where(p => p.Name == "A").Single();
            var result = requiredStrategies.IsRequired(p, SymbolType.Argument);
            result.Should().BeTrue();
        }

        [Fact]
        public void Attribute_strategy_finds_no_ArgumentRequired_on_parameter()
        {
            var p = typeof(StrategyTests).GetMethod("OptionSample").GetParameters().Where(p => p.Name == "D").Single();
            var result = requiredStrategies.IsRequired(p, SymbolType.Option);
            result.Should().BeFalse();
        }

        [Fact]
        public void Attribute_strategy_finds_OptionRequired_on_property()
        {
            var p = typeof(OptionSampleClass).GetProperties().Where(p => p.Name == "B").Single();
            var resultOption = requiredStrategies.IsRequired(p, SymbolType.Option);
            var resultArgument = requiredStrategies.IsRequired(p, SymbolType.Argument);
            resultOption.Should().BeTrue();
            resultArgument.Should().BeFalse();
        }

        [Fact]
        public void Attribute_strategy_finds_no_OptionRequired_or_ArgumentRequired_on_property()
        {
            var p = typeof(OptionSampleClass).GetProperties().Where(p => p.Name == "C").Single();
            var resultOption = requiredStrategies.IsRequired(p, SymbolType.Option);
            var resultArgument = requiredStrategies.IsRequired(p, SymbolType.Argument);
            resultOption.Should().BeFalse();
            resultArgument.Should().BeFalse();
        }

        [Fact]
        public void Attribute_strategy_finds_ArgumentRequired_on_property()
        {
            var p = typeof(OptionSampleClass).GetProperties().Where(p => p.Name == "A").Single();
            var resultOption = requiredStrategies.IsRequired(p, SymbolType.Option);
            var resultArgument = requiredStrategies.IsRequired(p, SymbolType.Argument);
            resultOption.Should().BeFalse();
            resultArgument.Should().BeTrue();
        }


        public void OptionSample([CmdOption(Name = "A2", Description = "Sue", OptionRequired = true, ArgumentRequired = true)] string A, string BArgument, string C, [CmdOption()] string D) { }
        public void ArgumentSample([CmdArgument(Name = "A2", Description = "Joe")] string A, string BArgument, string C, [CmdArgument()] string D) { }
        public void CommandSample([CmdCommand(Name = "A2", Description = "Sam")] string A, string BCommand, string C, [CmdCommand()] string D) { }
        public void AritySample([CmdArity(MinArgCount = 1, MaxArgCount = 3)] string A, string C) { }
        public void DescriptionSample([Description("Fred")] string A, string C) { }
        public void NameSample([CmdName("Terry")] string A, string C) { }

        public class ArgumentSampleClass
        {
            [CmdArgument]
            public string A { get; set; }
            public string BArgument { get; set; }
            public string C { get; set; }
        }

        public class OptionSampleClass
        {
            [CmdOption(ArgumentRequired = true)]
            public string A { get; set; }
            [CmdOption(OptionRequired = true)]
            public string B { get; set; }
            [CmdOption()]
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

        [Description("Frank")]
        [CmdName("Charlie")]
        public class DescriptionSampleClass
        {
            [Description("Fred")]
            [CmdName("Terry")]
            public string A { get; set; }
            public string C { get; set; }

            [CmdArgument(Description = "Mary", Name = "D2")]
            public string D { get; set; }

            [CmdArgument()]
            public string E { get; set; }

            [CmdOption(Description = "Jane", Name = "F2")]
            public string F { get; set; }

            [CmdOption()]
            public string G { get; set; }

            [CmdCommand(Description = "Jill", Name = "H2")]
            public string H { get; set; }

            [CmdCommand()]
            public string I { get; set; }

        }

        [CmdCommand(Name = "Jean", Description = "Will")]
        public class DescriptionSampleClass2
        {
        }

        public class DescriptionSampleClass3
        {
        }
    }
}
