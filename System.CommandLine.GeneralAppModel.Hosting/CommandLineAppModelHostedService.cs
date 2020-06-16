using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.ReflectionAppModel;
using System.Threading;
using System.Threading.Tasks;
using System.CommandLine.Parsing;

namespace System.CommandLine.GeneralAppModel.Hosting
{
    public class CommandLineAppModelHostedService<TCommand> : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly Strategy _strategy;
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();


        public CommandLineAppModelHostedService(ILogger<CommandLineAppModelHostedService<TCommand>> logger,
                                                IHostApplicationLifetime lifetime,
                                                IDescriptorMakerContext descriptorMakerContext = null)
        {
            _logger = logger;
            _lifetime = lifetime;
            _strategy = descriptorMakerContext?.Strategy is null
                          ? Strategy.Standard
                          : descriptorMakerContext.Strategy;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            //var arg = Environment.CommandLine;
            var arg = "Playground start Update start2 baz";
            var result = GetParseResult(arg);

            // Store the task we're executing
            _executingTask = ExecuteAsync(result, _stoppingCts.Token);

            // If the task is completed then return it, this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        private ParseResult GetParseResult(string arg)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor<TCommand>(_strategy);
            var builder = new CommandLineBuilder();
            CommandMaker.FillCommand(builder.Command, descriptor);
            return builder.Build().Parse(arg);
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }

        }

        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }

        private async Task ExecuteAsync(ParseResult result, CancellationToken cancellationTokenSource)
        {
            await result.InvokeAsync();
            _lifetime.StopApplication();
        }

    }
}