using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Settings;
using MySql.Data.MySqlClient;

namespace Mmcc.Stats.Infrastructure.Services.DataAccess
{
    public class TokensService : ITokensService
    {
        private readonly ILogger<TokensService> _logger;
        private readonly MySqlConnection _connection;
        
        public TokensService(ILogger<TokensService> logger, IOptions<DatabaseSettings> options)
        {
            _logger = logger;
            _connection = new MySqlConnection(options.Value.ToString());
        }

        public async Task<Token> GetToken(string value)
        {
            const string sql =
                "select id, value, clientname from clienttokens where value = @value;";
            return await _connection.QuerySingleOrDefaultAsync<Token>(sql, new {value});
        }
    }
}