using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;

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
            var serverPlayerbaseDataList = new List<ServerPlayerbaseData>();

            foreach (var server in servers)
            {
                var serverData = new ServerPlayerbaseData
                {
                    ServerName = server.ServerName,
                    TimesList = new List<DateTime>(),
                    PlayersOnlineList = new List<int>()
                };
                var pings = await _pingService.SelectPingsByServerAndDateAsync(server.ServerId, fromDate, toDate);
                

                foreach (var ping in pings)
                {
                    serverData.TimesList.Add(ping.PingTime);
                    serverData.PlayersOnlineList.Add(ping.PlayersOnline);
                }
                
                serverPlayerbaseDataList.Add(serverData);
            }

            return serverPlayerbaseDataList;
        }
    }
}