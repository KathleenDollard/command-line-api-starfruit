using System.CommandLine.GeneralAppModel;
using Xunit;
using FluentAssertions;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.NamedAttributeRules.Tests.ModelCodeForTests.MissingPropertyNotBlockingOtherAttributes;

namespace System.CommandLine.NamedAttributeRules
{
    public class SpecialScenariosTests
    {
        [Fact()]
        public void MissingPropertyNotBlockingOtherAttributes()
        {
            var strategy = new Strategy();
            strategy.CommandRules.DescriptionRules
                .Add(new NamedAttributeWithPropertyValueRule<string>("Command", "Description", SymbolType.Command))
                .Add(new NamedAttributeWithPropertyValueRule<string>("Description", "Description"))
                ;
            var actual = ReflectionDescriptorMaker
                .RootCommandDescriptor(strategy, typeof(SpecialScenarioModels));
            actual.Description.Should().Be("Boo!");

        }
    }
}
