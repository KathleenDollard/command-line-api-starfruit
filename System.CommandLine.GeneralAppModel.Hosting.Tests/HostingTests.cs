using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.GeneralAppModel.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Xunit;
using System.Threading.Tasks;
using System.CommandLine.GeneralAppModel;

namespace System.CommandLine.Hosting.Tests
{
    public static class HostingTests
    {
        private static string checkValue = null;

        [Fact]
        public static async Task CanExecuteCommandWithSimpleHost()
        {
            var args = new string[] { };
            await GeneralAppModelHosting.RunAsync<TestModel>(args);

            checkValue.Should().Be("Fred");
        }

        [Fact]
        public static async Task CanExecuteCommandWithConfiguredHost()
        {
            var args = new string[] { };
            IHostBuilder hostBuilder = Configure<TestModel>(args);
            await hostBuilder.Build().RunAsync();

            checkValue.Should().Be("Fred");

            static IHostBuilder ConfigureHost<TCommand>(Strategy strategy, string[] args)
            {
                return Host.CreateDefaultBuilder(args)
                                     .ConfigureServices((hostContext, services) =>
                                     {
                                         // Additional configuration
                                         services.AddHostedService<CommandLineAppModelHostedService<TCommand>>();
                                     });
            }

            static IHostBuilder Configure<TCommand>(string[] args)
            {
                return ConfigureHost<TCommand>(Strategy.Standard, args);
            }
        }


        public class TestModel
        {
            public void Invoke()
            {
                checkValue = "Fred";
            }
        }
    }
}