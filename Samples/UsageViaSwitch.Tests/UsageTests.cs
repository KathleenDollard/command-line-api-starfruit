using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.Samples;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class UsageViaIvokeTests
    {
        [Fact]
        public void InvokingMainReturnsSeven()
        {
            var arg = "start update start2 --allow-prerelease";
            var x = Samples.Program.Main(arg.Split(' '));
            x.Should().Be(7);
        }

        [Theory]
        //[InlineData("find", VerbosityLevel.Normal, "")]
        //[InlineData("--verbosity quiet find ", VerbosityLevel.Quiet,"")]
        //[InlineData("starting find", VerbosityLevel.Normal, "starting")]
        //[InlineData("starting --verbosity minimal find", VerbosityLevel.Minimal, "starting")]
        //[InlineData("--verbosity minimal starting find", VerbosityLevel.Minimal, "starting")]
        //[InlineData("starting -v minimal find", VerbosityLevel.Minimal, "starting")]
        [InlineData("-v minimal starting find", VerbosityLevel.Minimal, "starting")]
        public void ArgsForFindBuildCorrectInstance(string args, VerbosityLevel verbosity, string startPath)
        {
            var typedArg = new CommandLineActivator().CreateInstance<ManageGlobalJson>(args);
            using var _ = new AssertionScope();
            typedArg.Should().BeOfType<ManageGlobalJson.Find>();
            var x = typedArg ?? throw new InvalidOperationException("We just checked for null, how did this happen?");
            typedArg.Verbosity.Should().Be(verbosity);
            if (string.IsNullOrEmpty(startPath))
            {
                typedArg.StartPathArg.Should().BeNull();
            }
            else
            {
                typedArg.StartPathArg.Name.Should().Be(startPath);
            }
        }
    }
}
