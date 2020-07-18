using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;

namespace Mmcc.Stats.Infrastructure.HostedServices
{
    public class PollerTimedHostedService : IHostedService, IDisposable
    {
        private int _executionCount = 0;
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
            _logger.LogInformation("Poller Timed Hosted Service is starting...");
            
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            Interlocked.Increment(ref _executionCount);
            
            _logger.LogInformation("Poller Timed Hosted Service: Polling...");

            using var scope = Services.CreateScope();
            var scopedProcessingService = 
                scope.ServiceProvider
                    .GetRequiredService<IPollerService>();

            await scopedProcessingService.Poll();
            
            _logger.LogInformation("Poller Timed Hosted Service: Polling has completed.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Poller Timed Hosted Service is stopping...");

            _timer.Change(Timeout.Infinite, 0);
            
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}