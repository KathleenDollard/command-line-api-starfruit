using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using System.CommandLine;
using System.CommandLine.Samples;
using System.CommandLine.ReflectionAppModel;

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

        /// <summary>
        /// The strategy report let's you know what features are available as you 
        /// build your model - what rules are in play
        /// </summary>
        [Fact]
        public void StrategyReportShouldBeLong()
        {
            var strategy = new Strategy().SetStandardRules();
            var report = strategy.Report();
            report.Length.Should().BeGreaterThan(1000);
        }

        /// <summary>
        /// The strategy report let's you know what features are available as you 
        /// build your model - what rules are in play
        /// </summary>
        [Fact]
        public void MarkdownStrategyReportShouldBeLong()
        {
            var strategy = new Strategy().SetStandardRules();
            var report = strategy.Report( Strategy.ReportFormat.Markdown, VerbosityLevel.Minimal  );
            report.Length.Should().BeGreaterThan(1000);
        }

        /// <summary>
        /// The Descriptor report describes the interim model that captures your intent 
        /// </summary>
        [Fact]
        public void DescriptorReportShouldBeLong()
        {
            var strategy = new Strategy().SetStandardRules();
            var activator = new CommandLineActivator();
            var descriptor = activator.GetCommandDescriptor<ManageGlobalJson>(strategy);
            var report = descriptor.Report(0, VerbosityLevel.Detailed );
            report.Length.Should().BeGreaterThan(1000);
        }

        /// <summary>
        /// If we create a Symbol report it would describe the System.CommandLine model
        /// created. 
        /// generation. 
        /// </summary>
        /// <remarks>
        /// While this would be useful, it would probably have to be against an
        /// in memory object, and may be hard (or possibly easy) to do with source.
        /// <br/>
        /// All of the user impact will be captured in the descriptor. Rules, which 
        /// may result in incorrect user intent (user bugs) will be in the descriptor.
        /// <br/>
        /// Put a different way, Descriptor bugs may be user bugs. Symbol/CommandLine
        /// bugs are the fault of the AppModel, or possibly user misunderstanding what
        /// is contained in the AppModel. For example, nailing down "Name"
        /// </remarks>
        [Fact]
        public void SymbolReportMayBeUseful()
        {
  
        }


        /// <summary>
        /// We may want a log of what rules are applied to create the Descriptor. It's one thing
        /// to look at the rules/strategy and predict how your code created the descriptors, 
        /// and another to actually have a log of which rules were used. 
        /// </summary>
        [Fact]
        public void LogMightBeUseful()
        {
         
        }



    }
}
