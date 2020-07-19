using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Settings;
using MySql.Data.MySqlClient;

namespace Mmcc.Stats.Infrastructure.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly MySqlConnection _connection;

        public DatabaseService(IOptions<DatabaseSettings> options)
        {
            _connection = new MySqlConnection(options.Value.ToString());
        }

        public async Task<bool> DoesTableExistAsync(string name)
        {
            const string sql =
                "SELECT count(*) FROM information_schema.TABLES WHERE TABLE_NAME = @name;";
            var q = await _connection.QueryFirstOrDefaultAsync<int>(sql, new {name});
            return q != 0;
        }

        public async Task<IEnumerable<Server>> SelectServersAsync()
        {
            const string sql =
                "select serverId, serverIp, serverPort, serverName from server;";
            return await _connection.QueryAsync<Server>(sql);
        }

        public async Task<IEnumerable<Server>> SelectEnabledServersAsync()
        {
            const string sql =
                "select serverId, serverIp, serverPort, serverName from server where enabled = 1;";
            return await _connection.QueryAsync<Server>(sql);
        }
        
        public async Task<IEnumerable<Ping>> SelectPingsByServerAndDateAsync(int serverId, DateTime fromDate, DateTime toDate)
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