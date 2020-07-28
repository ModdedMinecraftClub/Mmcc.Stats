using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Mmcc.Stats.Infrastructure.Services
{
    public class PlayerbaseService : IPlayerbaseService
    {
        private readonly ILogger<PlayerbaseService> _logger;
        private readonly IPingService _pingService;
        private readonly IServerService _serverService;

        public PlayerbaseService(ILogger<PlayerbaseService> logger, IPingService pingService,
            IServerService serverService)
        {
            _logger = logger;
            _pingService = pingService;
            _serverService = serverService;
        }

        public async Task<IEnumerable<ServerPlayerbaseData>> GetByDateAsync(DateTime fromDate, DateTime toDate)
            => await Task.WhenAll((await _serverService.SelectServersAsync())
                .GroupBy(x => x.ServerId)
                .Select(async y =>
                {
                    var pings = await _pingService.SelectPingsByServerAndDateAsync(y.Key, fromDate, toDate);
                    return new ServerPlayerbaseData
                    {
                        ServerName = y.First().ServerName,
                        Pings = pings.Select(ping => new PingDto
                        {
                            PlayersOnline = ping.PlayersOnline,
                            Time = ping.PingTime
                        })
                    };
                }));
    }
}