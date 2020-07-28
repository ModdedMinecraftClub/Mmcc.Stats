using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;

namespace Mmcc.Stats.Infrastructure.HostedServices
{
    public class StartupChecksHostedService : IHostedService
    {
        private readonly ILogger<StartupChecksHostedService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IHostEnvironment _environment;

        public IServiceProvider Services { get; }

        public StartupChecksHostedService(
            ILogger<StartupChecksHostedService> logger,
            IHostApplicationLifetime appLifetime,
            IHostEnvironment environment,
            IServiceProvider services
            )
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _environment = environment;
            Services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Environment: {_environment.EnvironmentName}");
            await CheckDbAsync();
        }

        private async Task CheckDbAsync()
        {
            _logger.LogInformation("Checking database tables...");

            using var scope = Services.CreateScope();
            var scopedDb = scope.ServiceProvider.GetRequiredService<IDbTableService>();

            await CheckIfTableExistsAsync("pings");
            await CheckIfTableExistsAsync("server");
            await CheckIfTableExistsAsync("tpsstats");

            _logger.LogInformation("Tables successfully found.");
            
            async Task CheckIfTableExistsAsync(string tableName)
            {
                var doesTableExist = await scopedDb.DoesTableExistAsync(tableName);

                if (!doesTableExist)
                {
                    _logger.LogCritical(
                        $"{tableName} table not found. Please ensure the table exists before starting the app. Exiting..."
                    );
                    _appLifetime.StopApplication();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}