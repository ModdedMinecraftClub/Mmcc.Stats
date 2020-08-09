using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Infrastructure.Services
{
    /// <summary>
    /// <inheritdoc cref="IPlayerbaseService"/>
    /// </summary>
    public class PlayerbaseService : IPlayerbaseService
    {
        private readonly ILogger<PlayerbaseService> _logger;
        private readonly IPingService _pingService;
        private readonly IServerService _serverService;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerbaseService"/> class. 
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="pingService">Ping service</param>
        /// <param name="serverService">Server service</param>
        public PlayerbaseService(ILogger<PlayerbaseService> logger, IPingService pingService,
            IServerService serverService)
        {
            _logger = logger;
            _pingService = pingService;
            _serverService = serverService;
        }
        
        /// <summary>
        /// Gets MC server playerbase data for all servers in a given time period.
        /// </summary>
        /// <param name="fromDate">From date</param>
        /// <param name="toDate">To date</param>
        /// <returns>An <see cref="IEnumerable{ServerPlayerbaseDate}"/> containing server playerbase data for all server servers in a given time period.</returns>
        public async Task<IEnumerable<ServerPlayerbaseData>> GetByDateAsync(DateTime fromDate, DateTime toDate)
            => (await _pingService.SelectPingsByDateAsync(fromDate, toDate))
                .GroupBy(ping => ping.ServerId)
                .Select(serverPing => new ServerPlayerbaseData
                {
                    ServerName = serverPing.First().ServerName,
                    Times = serverPing.Select(x => x.PingTime),
                    Players = serverPing.Select(x => (double) x.PlayersOnline)
                });
        
        /// <summary>
        /// Gets MC server playerbase data for all servers in a given time period with players online as rolling average.
        /// </summary>
        /// <param name="fromDate">From date</param>
        /// <param name="toDate">To date</param>
        /// <param name="windowSize">Window size for the rolling average</param>
        /// <returns>An <see cref="IEnumerable{ServerPlayerbaseDate}"/> containing server playerbase data for all
        /// server servers in a given time period with players online as a rolling average.</returns>
        public async Task<IEnumerable<ServerPlayerbaseData>> GetByDateAsRollingAvgAsync(
            DateTime fromDate, DateTime toDate, int windowSize)
        {
            var input = (await GetByDateAsync(fromDate, toDate)).ToList();
            var output = new List<ServerPlayerbaseData>();
            
            foreach (var server in input)
            {
                var serverNew = new ServerPlayerbaseData
                {
                    ServerName = server.ServerName,
                    Players = server.Players.MovingAverage(windowSize),
                    Times = server.Times
                };
                output.Add(serverNew);
            }

            return output;
        }
    }
}