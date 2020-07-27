using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IPingsService : IDisposable
    {
        Task<IEnumerable<Ping>> SelectPingsByServerAndDateAsync(int serverId, DateTime fromDate, DateTime toDate);
        Task InsertPingsAsync(IEnumerable<Ping> pings);
    }
}