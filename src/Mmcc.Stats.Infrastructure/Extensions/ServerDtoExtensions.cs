using Mmcc.Stats.Core.Data.Dtos;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Infrastructure.Extensions
{
    public static class ServerDtoExtensions
    {
        public static ServerDto ToServerDto(this Server server)
            => new ServerDto
            {
                ServerId = server.ServerId,
                ServerName = server.ServerName,
                Enabled = server.Enabled
            };
    }
}