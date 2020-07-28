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
    public class ServerService : IServerService
    {
        private readonly ILogger<ServerService> _logger;
        private readonly MySqlConnection _connection;

        public ServerService(ILogger<ServerService> logger, DatabaseSettings options)
        {
            _logger = logger;
            _connection = new MySqlConnection(options.ToString());
        }

        public async Task<Server> SelectServer(int serverId)
        {
            const string sql =
                "select serverId, serverIp, serverPort, serverName from server where serverId = @serverId;";
            return await _connection.QuerySingleOrDefaultAsync(sql, new {serverId});
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

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}