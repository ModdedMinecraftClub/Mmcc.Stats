using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Core
{
    public interface IServersPlayerbaseService
    {
        Task<IEnumerable<ServerPlayerbaseData>> GetByDate(DateTime fromDate, DateTime toDate);
    }
}