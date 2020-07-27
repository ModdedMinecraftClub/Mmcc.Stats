using System;
using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface ITpsService : IDisposable
    {
        Task InsertTpsStatAsync(TpsStat tpsStat);
    }
}