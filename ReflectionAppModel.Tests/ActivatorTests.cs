using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests.TypedAttributes;
using System.Text;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Linq;

namespace System.CommandLine.ReflectionAppModel.Tests
{
    public partial class ActivatorTests
    {
        private string commandName = "MyCommand";
        private readonly Strategy fullStrategy;
        private readonly Strategy standardStrategy;
        private const string full = "Full";
        private const string standard = "Standard";

        public ActivatorTests()
        {
            fullStrategy = Strategy.Full;
            standardStrategy = Strategy.Standard ;
        }

        [Theory()]
        [InlineData("A", false)]
        [InlineData("A --allow", true)]
        public void CommandWithSubCommands(string args, bool allow)
        {
            // args[0] = $"{commandName} {args[0]}";
            var instance = new CommandLineActivator().CreateInstance<TypeWithTwoCommandsByDerivedType>(args, commandName: commandName);
            using var scope = new AssertionScope();
            instance.Should().BeOfType<TypeWithTwoCommandsByDerivedType.A>();
            var a = instance as TypeWithTwoCommandsByDerivedType.A;
            var _ = a ?? throw new InvalidOperationException(); ;
            a.Allow.Should().Be(allow);
        }

        [Theory()]
        [InlineData("A", standard )]
        public void ArgumentEndingWithArgIsSet(string args, string strategyName )
        {
            var strategy = strategyName == full ? fullStrategy : standardStrategy;
            var descriptor =  ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeof(TypeWithOneArgumentByArgName));
            var instance = new CommandLineActivatorForTests(descriptor).CreateInstance<TypeWithOneArgumentByArgName>(args, commandName: commandName);
            using var scope = new AssertionScope();
            descriptor.Arguments.First().Name.Should().Be("Red");
            instance.Should().BeOfType<TypeWithOneArgumentByArgName>();
            var _ = instance ?? throw new InvalidOperationException(); 
            instance.RedArg.Should().Be(args);
        }
    }
}
