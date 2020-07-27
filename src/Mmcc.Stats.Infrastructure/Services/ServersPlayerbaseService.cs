using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Infrastructure.Services
{
    public class ServersPlayerbaseService : IServersPlayerbaseService
    {
        private readonly ILogger<ServersPlayerbaseService> _logger;
        private readonly IPingsService _pingsService;
        private readonly IServersService _serversService;

        public ServersPlayerbaseService(ILogger<ServersPlayerbaseService> logger, IPingsService pingsService, IServersService serversService)
        {
            _logger = logger;
            _pingsService = pingsService;
            _serversService = serversService;
        }

        public async Task<IEnumerable<ServerPlayerbaseData>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var servers = await _serversService.SelectServersAsync();
            var serverPlayerbaseDataList = new List<ServerPlayerbaseData>();

            foreach (var server in servers)
            {
                var serverData = new ServerPlayerbaseData
                {
                    ServerName = server.ServerName,
                    TimesList = new List<DateTime>(),
                    PlayersOnlineList = new List<int>()
                };
                var pings = await _pingsService.SelectPingsByServerAndDateAsync(server.ServerId, fromDate, toDate);
                

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