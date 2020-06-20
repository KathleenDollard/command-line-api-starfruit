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
    public class ActivatorTests
    {
        private string commandName = "MyCommand";

        [Theory(Skip ="Currently failing")]
        [InlineData("typewithtwocommandsbyderivedtype a")]
        public void CommandWithSubCommands(params string[] args)
        {
           // args[0] = $"{commandName} {args[0]}";
            var result = CommandLineActivator.CreateInstance<TypeWithTwoCommandsByDerivedType>(args, commandName:commandName);
            using var scope = new AssertionScope();
            result.Should().BeOfType<TypeWithTwoCommandsByDerivedType.A>();
            var a = result as TypeWithTwoCommandsByDerivedType.A;
            var _ = a ?? throw new InvalidOperationException(); ;
            a.Allow.Should().BeTrue();
        }
    }
}
