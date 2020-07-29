using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IServerTpsService
    {
        Task<IEnumerable<ServerTpsData>> GetByDateAsync(DateTime fromDate, DateTime toDate);
        Task ProcessIncomingTps(McTpsStatDto tpsStat);
    }
}