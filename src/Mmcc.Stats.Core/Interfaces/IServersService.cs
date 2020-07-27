using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IServersService : IDisposable
    {
        Task<IEnumerable<Server>> SelectServersAsync();
        Task<IEnumerable<Server>> SelectEnabledServersAsync();
    }
}