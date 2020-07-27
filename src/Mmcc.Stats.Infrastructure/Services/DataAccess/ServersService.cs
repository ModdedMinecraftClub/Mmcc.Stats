using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Settings;
using MySql.Data.MySqlClient;

namespace Mmcc.Stats.Infrastructure.Services.DataAccess
{
    public class ServersService : IServersService
    {
        private readonly ILogger<ServersService> _logger;
        private readonly MySqlConnection _connection;

        public ServersService(ILogger<ServersService> logger, IOptions<DatabaseSettings> options)
        {
            _logger = logger;
            _connection = new MySqlConnection(options.Value.ToString());
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