using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionModel.Tests.DotnetModel;
using System.Text;
using Xunit;

namespace System.CommandLine.ReflectionModel.Tests
{
    public class DotnetModelTests
    {
        private readonly Strategy strategy;
        public DotnetModelTests()
        {
            strategy = new Strategy()
                                .SetStandardRules();
            strategy.SelectSymbolRules.NamesToIgnore.AddRange(new string[]{ "CommandDataFromMethods", "CommandDataFromType"});
        }

        [Fact]
        public void CanMakeDotnetInstall()
            => Utils.TestType<Install>(strategy);
    }
}
