using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using MySqlConnector;
using TraceLd.MineStatSharp;

namespace Mmcc.Stats.Infrastructure.Services
{
    public class PollerService : IPollerService
    {
        private readonly ILogger<PollerService> _logger;
        private readonly IPingService _pingService;
        private readonly IServerService _serverService;

        public PollerService(IPingService pingService, ILogger<PollerService> logger, IServerService serverService)
        {
            _pingService = pingService;
            _logger = logger;
            _serverService = serverService;
        }

        public async Task PollAsync()
        {
            try
            {
                var pings = new LinkedList<Ping>();
                var activeServers = (await _serverService.SelectEnabledServersAsync()).ToList();

                if (!activeServers.Any())
                {
                    _logger.LogWarning("Poller: No active servers detected.");
                }

                foreach (var activeServer in activeServers)
                {
                    var ms = new MineStat(activeServer.ServerIp, (ushort) activeServer.ServerPort);

                    if (ms.ServerUp)
                    {
                        var ping = new Ping
                        {
                            ServerId = activeServer.ServerId,
                            PingTime = DateTime.Now,
                            PlayersOnline = int.Parse(ms.CurrentPlayers),
                            PlayersMax = int.Parse(ms.MaximumPlayers)
                        };
                        
                        pings.AddFirst(ping);
                    }
                    else
                    {
                        _logger.LogWarning(
                            $"Poller: Server {activeServer.ServerId} with IP address {activeServer.ServerIp}:{activeServer.ServerPort} is down.");
                    }
                }

                await _pingService.InsertPingsAsync(pings);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case MySqlException _:
                        _logger.LogCritical(ex, ex.Message);
                        break;
                    default:
                        _logger.LogError(ex, $"Poller: {ex.Message}");
                        break;
                }
            }
        }
    }
}