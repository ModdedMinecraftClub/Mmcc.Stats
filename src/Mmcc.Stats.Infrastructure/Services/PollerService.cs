using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models;
using Mmcc.Stats.Core.Interfaces;
using TraceLd.MineStatSharp;

namespace Mmcc.Stats.Infrastructure.Services
{
    /// <summary>
    /// <inheritdoc cref="IPollerService"/>
    /// </summary>
    public class PollerService : IPollerService
    {
        private readonly ILogger<PollerService> _logger;
        private readonly PollerContext _context;

        /// <summary>
        /// <inheritdoc cref="IPollerService"/>
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="context">Poller database context</param>
        public PollerService(ILogger<PollerService> logger, PollerContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Polls all active MC servers for playerbase statistics.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation</returns>
        public async Task PollAsync()
        {
            var pings = new LinkedList<Ping>();
            var activeServers = _context.Servers.AsNoTracking()
                .Where(server => server.Enabled);

            if (!activeServers.Any())
            {
                _logger.LogWarning($"[{nameof(PollerService)}] No active servers detected.");
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

            await _context.Pings.AddRangeAsync(pings);
            await _context.SaveChangesAsync();
        }
    }
}