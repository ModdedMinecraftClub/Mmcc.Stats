using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;
using Mmcc.Stats.Core.Models.Settings;
using MySqlConnector;

namespace Mmcc.Stats.Infrastructure.Services.DataAccess
{
    public class PingService : IPingService

    {
        private readonly ILogger<PingService> _logger;
        private readonly MySqlConnection _connection;

        public PingService(ILogger<PingService> logger, DatabaseSettings options)
        {
            _logger = logger;
            _connection = new MySqlConnection(options.ToString());
        }

        public async Task<IEnumerable<ServerPingDto>> SelectPingsByDateAsync(DateTime fromDate, DateTime toDate)
        {
            const string sql =
                @"SELECT server.serverId, server.serverName, pings.pingTime, pings.playersOnline
              FROM pings
              INNER JOIN server ON pings.serverId = server.serverId
              WHERE pingTime >= @fromDate and pingTime <= @toDate
              ORDER BY pings.pingTime;";
            var payload = new
            {
                fromDate,
                toDate
            };
            
            return await _connection.QueryAsync<ServerPingDto>(sql, payload);
        }

        public async Task<IEnumerable<Ping>> SelectPingsByServerAndDateAsync(int serverId, DateTime fromDate,
            DateTime toDate)
        {
            const string sql =
                "select serverId, pingTime, playersOnline, playersMax from pings where serverId = @serverId and pingTime >= @fromDate and pingTime <= @toDate;";

            var payload = new
            {
                serverId,
                fromDate,
                toDate
            };

            return await _connection.QueryAsync<Ping>(sql, payload);
        }

        public async Task InsertPingsAsync(IEnumerable<Ping> pings)
        {
            const string sql =
                "insert into pings (serverId, pingTime, playersOnline, playersMax) VALUES (@serverId, @pingTime, @playersOnline, @playersMax);";
            await _connection.ExecuteAsync(sql, pings);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}