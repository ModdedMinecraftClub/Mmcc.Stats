using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Settings;
using MySqlConnector;

namespace Mmcc.Stats.Infrastructure.Services.DataAccess
{
    public class TpsService : ITpsService
    {
        private readonly ILogger<TpsService> _logger;
        private readonly MySqlConnection _connection;

        public TpsService(ILogger<TpsService> logger, DatabaseSettings options)
        {
            _logger = logger;
            _connection = new MySqlConnection(options.ToString());
        }

        public async Task<IEnumerable<TpsStat>> SelectTpsByServerAndDateAsync(int serverId, DateTime fromDate, DateTime toDate)
        {
            const string sql =
                "select serverId, statTime, tps from tpsstats where serverId = @serverId and statTime >= @fromDate and statTime <= @toDate;";
            var payload = new
            {
                serverId,
                fromDate,
                toDate
            };
            return await _connection.QueryAsync<TpsStat>(sql, payload);
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