using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IPlayerbaseService
    { 
        Task<IEnumerable<ServerPlayerbaseData>> GetByDateAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ServerPlayerbaseData>> GetByDateAsRollingAvgAsync(DateTime fromDate, DateTime toDate, int windowSize);
    }
}