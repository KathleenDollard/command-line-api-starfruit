using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.CommandLine.GeneralAppModel.Hosting;
using System.CommandLine.GeneralAppModel;

namespace Playground.PotentialPatterns
{
    static class MainWithGeneraicHostInvokeWithConfiguration
    {
        public static async Task Main1(string[] args)
        {
            await Configure<ManageGlobalJson>(args).Build().RunAsync();
        }

        private static IHostBuilder Configure<TCommand>(Strategy strategy, string[] args)
          => Host.CreateDefaultBuilder(args)
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddHostedService<CommandLineAppModelHostedService<TCommand>>();
              })
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddConsole();
              });

        private static IHostBuilder Configure<TCommand>(string[] args)
           => Configure<TCommand>(Strategy.Standard, args);

    }
}
