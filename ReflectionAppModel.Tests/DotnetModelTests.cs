using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel.Tests.DotnetModel;
using Xunit;

namespace System.CommandLine.ReflectionAppModel.Tests
{
    public class DotnetModelTests
    {
        private readonly Strategy strategy;
        public DotnetModelTests()
        {
            strategy = new Strategy()
                           .SetReflectionRules()
                           .SymbolCandidateNamesToIgnore( "CommandDataFromMethods", "CommandDataFromType");
        }

        //[Fact]
        //public void CanMakeDotnetInstall()
        //    => Utils.TestType<Install>(strategy);

        [Fact]
        public void CanMakeDotnet()
            => Utils.TestType<Dotnet>(strategy);
    }
}
