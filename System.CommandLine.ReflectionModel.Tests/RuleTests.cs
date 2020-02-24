using FluentAssertions;
using Xunit;

namespace System.CommandLine.ReflectionModel.Tests
{

    public class RuleTests
    {
        private readonly CommandMaker commandMaker;

        public RuleTests()
        {
            commandMaker = new CommandMaker();
            commandMaker.UseDefaults();
        }

        [Fact]
        public void Default_strategies_are_as_expected()
        {
            var rules = commandMaker.StrategiesSet.StrategiesSetDescription;
            rules.Should().Be(@"
AppModel:
   IsArgumentStrategies: 
       Attribute: [CmdArgumentAttribute] 
       String Contents: Suffix - 'Arg'
       String Contents: Suffix - 'Argument'
   IsCommandStrategies:
       Attribute: [CmdCommandAttribute] 
       String Contents: Suffix - 'Command'
       Complex Type Strategy
   SubCommandStrategies:
       Type Strategy: Derived Types
   ArityStrategies:
       Attribute: [CmdArityAttribute] new ArityDescriptor(attribute.MinArgCount, attribute.MaxArgCount)
   DescriptionStrategies:
       Attribute: [DescriptionAttribute] attribute.Description
       Attribute: [CmdCommandAttribute] attribute.Description
       Attribute: [CmdArgOptionBaseAttribute] attribute.Description
       XML Documentation Strategy
   NameStrategies:
       Attribute: [CmdNameAttribute] attribute.Name
       Attribute: [CmdArgOptionBaseAttribute] attribute.Name
       Attribute: [CmdCommandAttribute] attribute.Name
       Attribute: [CmdArgOptionBaseAttribute] attribute.Name
   IsRequiredStrategies:
       Attribute: [CmdRequiredAttribute] 
       Attribute: [CmdOptionAttribute] 
       Attribute: [CmdOptionAttribute] 
       Attribute: [CmdArgumentAttribute] ");
        }
    }
}
