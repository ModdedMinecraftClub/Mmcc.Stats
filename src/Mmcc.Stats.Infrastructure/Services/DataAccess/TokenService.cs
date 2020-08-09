using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Settings;
using MySqlConnector;

namespace Mmcc.Stats.Infrastructure.Services.DataAccess
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly MySqlConnection _connection;
        
        public TokenService(ILogger<TokenService> logger, DatabaseSettings options)
        {
            _logger = logger;
            _connection = new MySqlConnection(options.ToString());
        }

        public async Task<Token> GetToken(string value)
        {
            const string sql =
                "select id, value, clientname from clienttokens where value = @value;";
            return await _connection.QuerySingleOrDefaultAsync<Token>(sql, new {value});
        }
    }
}