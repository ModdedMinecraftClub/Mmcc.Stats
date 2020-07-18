using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Infrastructure.Services
{
    public interface IPlayerbaseStatsService
    {
        Task<IEnumerable<ServerPlayerbaseData>> GetRaw(DateTime fromDate, DateTime toDate);
    }
    
    public class PlayerbaseStatsService : IPlayerbaseStatsService
    {
        private readonly ILogger<PlayerbaseStatsService> _logger;
        private readonly IDatabaseService _db;

        public PlayerbaseStatsService(ILogger<PlayerbaseStatsService> logger, IDatabaseService db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IEnumerable<ServerPlayerbaseData>> GetRaw(DateTime fromDate, DateTime toDate)
        {
            var servers = await _db.SelectServers();
            var serverPlayerbaseDataList = new List<ServerPlayerbaseData>();

            foreach (var server in servers)
            {
                var serverData = new ServerPlayerbaseData
                {
                    ServerName = server.ServerName,
                    Data = new List<ServerPlayerbaseDataPoint>()
                };
                var pings = await _db.SelectPingsByServerAndDate(server.ServerId, fromDate, toDate);
                
                foreach (var ping in pings)
                {
                    var dataPoint = new ServerPlayerbaseDataPoint
                    {
                        Time = ping.PingTime,
                        PlayersOnline = ping.PlayersOnline
                    };

                    serverData.Data.Add(dataPoint);
                }
                
                serverPlayerbaseDataList.Add(serverData);
            }

            return serverPlayerbaseDataList;
        }
    }
}