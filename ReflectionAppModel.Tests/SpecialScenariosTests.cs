using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using A = System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests.MissingPropertyNotBlockingOtherAttributes;
using System.Text;
using Xunit;
using FluentAssertions;

namespace System.CommandLine.ReflectionAppModel
{
    public class SpecialScenariosTests
    {
        [Fact()]
        public void MissingPropertyNotBlockingOtherAttributes()
        {
            var strategy = new Strategy();
            strategy.CommandRules.DescriptionRules
                .Add(new NamedAttributeWithPropertyRule<string>("Command", "Description", SymbolType.Command))
                .Add(new NamedAttributeWithPropertyRule<string>("Description", "Description"))
                ;
            var actual = ReflectionDescriptorMaker
                .RootCommandDescriptor(strategy, typeof(A.SpecialScenarioModels));
            actual.Description.Should().Be("Boo!");

        }
    }
}
