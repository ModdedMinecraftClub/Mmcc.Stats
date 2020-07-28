﻿using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models.Settings;
using MySql.Data.MySqlClient;

namespace Mmcc.Stats.Infrastructure.Services.DataAccess
{
    public class DbTableService : IDbTableService
    {
        private readonly ILogger<DbTableService> _logger;
        private readonly MySqlConnection _connection;

        public DbTableService(ILogger<DbTableService> logger, DatabaseSettings options)
        {
            _logger = logger;
            _connection = new MySqlConnection(options.ToString());
        }
        
        public async Task<bool> DoesTableExistAsync(string name)
        {
            const string sql =
                "SELECT count(*) FROM information_schema.TABLES WHERE TABLE_NAME = @name;";
            var q = await _connection.QueryFirstOrDefaultAsync<int>(sql, new {name});
            return q != 0;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}