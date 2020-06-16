using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace System.CommandLine.GeneralAppModel.Hosting
{
    public static class GeneralAppModelHosting
    {

        public static IHostBuilder Configure<TCommand>(Strategy strategy, string[] args)
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

        public static IHostBuilder Configure<TCommand>(string[] args)
           => Configure<TCommand>(Strategy.Standard, args);

        public static async Task RunAsync<TCommand>(string[] args)
            => await Configure<TCommand>(args).Build().RunAsync();
    }
}