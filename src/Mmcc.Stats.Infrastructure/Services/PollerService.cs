using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using MySql.Data.MySqlClient;
using TraceLd.MineStatSharp;

namespace Mmcc.Stats.Infrastructure.Services
{
    public class PollerService : IPollerService
    {
        private readonly ILogger<PollerService> _logger;
        private readonly IPingsService _pingsService;
        private readonly IServersService _serversService;

        public PollerService(IPingsService pingsService, ILogger<PollerService> logger, IServersService serversService)
        {
            _pingsService = pingsService;
            _logger = logger;
            _serversService = serversService;
        }

        public async Task PollAsync()
        {
            try
            {
                var pings = new LinkedList<Ping>();
                var activeServers = (await _serversService.SelectEnabledServersAsync()).ToList();

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

                await _pingsService.InsertPingsAsync(pings);
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