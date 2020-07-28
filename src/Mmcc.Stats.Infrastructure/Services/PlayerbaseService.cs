using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Mmcc.Stats.Infrastructure.Services
{
    public class PlayerbaseService : IPlayerbaseService
    {
        private readonly ILogger<PlayerbaseService> _logger;
        private readonly IPingService _pingService;
        private readonly IServerService _serverService;

        public PlayerbaseService(ILogger<PlayerbaseService> logger, IPingService pingService, IServerService serverService)
        {
            _logger = logger;
            _pingService = pingService;
            _serverService = serverService;
        }

        public async Task<IEnumerable<ServerPlayerbaseData>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var servers = await _serverService.SelectServersAsync();
            var queryTasks =
                servers.GroupBy(x => x.ServerId)
                    .Select(async y =>
                    {
                        var pings = (await _pingService.SelectPingsByServerAndDateAsync(y.Key, fromDate, toDate)).ToList();
                        return new ServerPlayerbaseData
                        {
                            ServerName = y.First().ServerName,
                            TimesList = pings.Select(ping => ping.PingTime).ToList(),
                            PlayersOnlineList = pings.Select(ping => ping.PlayersOnline).ToList()
                        };
                    });
            var output = await Task.WhenAll(queryTasks);
            return output;
        }
    }
}