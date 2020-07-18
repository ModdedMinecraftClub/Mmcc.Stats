using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Core
{
    public interface IDatabaseService : IDisposable
    {
        Task<bool> DoesTableExistAsync(string name);
        Task<IEnumerable<Server>> SelectServers();
        Task<IEnumerable<Server>> SelectEnabledServersAsync();
        Task<IEnumerable<Ping>> SelectPingsByServerAndDate(int serverId, DateTime fromDate, DateTime toDate);
        Task InsertPingsAsync(IEnumerable<Ping> pings);
    }
}