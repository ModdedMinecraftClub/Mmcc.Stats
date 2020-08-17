using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Interfaces;

namespace Mmcc.Stats.Infrastructure.HostedServices
{
    public class PollerTimedHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger<PollerTimedHostedService> _logger;

        public IServiceProvider Services { get; }
        
        public PollerTimedHostedService(
            IServiceProvider services,
            ILogger<PollerTimedHostedService> logger
        )
        {
            Services = services;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[{nameof(PollerTimedHostedService)}] Starting the service...");
            
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation($"[{nameof(PollerTimedHostedService)}] Polling...");

            using var scope = Services.CreateScope();
            var scopedProcessingService = 
                scope.ServiceProvider
                    .GetRequiredService<IPollerService>();

            try
            {
                await scopedProcessingService.PollAsync();
                _logger.LogInformation($"[{nameof(PollerTimedHostedService)}] Polling has completed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception has occurred while polling.");
                _logger.LogError($"[{nameof(PollerTimedHostedService)}] Polling has failed. Retrying in 10 minutes.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[{nameof(PollerTimedHostedService)}] Stopping the service.");
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}