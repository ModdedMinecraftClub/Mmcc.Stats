using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface ITpsService : IDisposable
    {
        Task<IEnumerable<TpsStat>> SelectTpsByServerAndDateAsync(int serverId, DateTime fromDate, DateTime toDate);
        Task InsertTpsStatAsync(TpsStat tpsStat);
    }
}