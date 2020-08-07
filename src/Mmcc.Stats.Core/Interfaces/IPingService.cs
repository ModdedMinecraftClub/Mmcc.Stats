using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IPingService : IDisposable
    {
        Task<IEnumerable<ServerPingDto>> SelectPingsByDateAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Ping>> SelectPingsByServerAndDateAsync(int serverId, DateTime fromDate, DateTime toDate);
        Task InsertPingsAsync(IEnumerable<Ping> pings);
    }
}