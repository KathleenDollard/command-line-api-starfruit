using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using System.CommandLine;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class UsageViaSwitchTests
    {
        [Fact]
        public void InvokingMainReturnsSeven()
        {
            var arg = "start update start2 --allow-prerelease";
            var x = Samples.Program.Main(arg.Split(' '));
            x.Should().Be(7);
        }


    }
}
