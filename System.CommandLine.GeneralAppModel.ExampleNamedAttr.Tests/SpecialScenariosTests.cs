using System.CommandLine.GeneralAppModel;
using Xunit;
using FluentAssertions;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests.MissingPropertyNotBlockingOtherAttributes;

namespace System.CommandLine.ReflectionAppModel
{
    public class SpecialScenariosTests
    {
        [Fact()]
        public void MissingPropertyNotBlockingOtherAttributes()
        {
            var strategy = new Strategy();
            strategy.CommandRules.DescriptionRules
                .Add(new AttributeWithPropertyValueRule<string>("Command", "Description", SymbolType.Command))
                .Add(new AttributeWithPropertyValueRule<string>("Description", "Description"))
                ;
            var actual = ReflectionDescriptorMaker
                .RootCommandDescriptor(strategy, typeof(SpecialScenarioModels));
            actual.Description.Should().Be("Boo!");

        }
    }
}
