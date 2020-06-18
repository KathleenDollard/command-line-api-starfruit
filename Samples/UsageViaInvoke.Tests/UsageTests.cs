using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class UsageViaSwitchTests
    {
        [Fact]
        public void InvokingMainReturnsSeven()
        {
            var arg = "start Update start2 --allow-prerelease";
            var x = Samples.Program.Main(arg.Split(' '));
            x.Should().Be(7);
        }


    }
}
