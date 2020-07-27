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
    public class TpsService : ITpsService
    {
        private readonly ILogger<TpsService> _logger;
        private readonly MySqlConnection _connection;

        public TpsService(ILogger<TpsService> logger, IOptions<DatabaseSettings> options)
        {
            _logger = logger;
            _connection = new MySqlConnection(options.Value.ToString());
        }
        
        public async Task InsertTpsStatAsync(TpsStat tpsStat)
        {
            const string sql =
                "insert into tpsstats (serverId, statTime, tps) VALUES (@serverId, @statTime, @tps);";
            await _connection.ExecuteAsync(sql, tpsStat);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}