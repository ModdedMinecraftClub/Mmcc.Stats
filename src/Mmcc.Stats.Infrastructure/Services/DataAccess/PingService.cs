using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Settings;
using MySql.Data.MySqlClient;

namespace Mmcc.Stats.Infrastructure.Services.DataAccess
{
    public class PingService : IPingService

    {
    private readonly ILogger<PingService> _logger;
    private readonly MySqlConnection _connection;

    public PingService(ILogger<PingService> logger, IOptions<DatabaseSettings> options)
    {
        _logger = logger;
        _connection = new MySqlConnection(options.Value.ToString());
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