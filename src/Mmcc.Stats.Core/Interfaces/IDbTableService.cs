using System;
using System.Threading.Tasks;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IDbTableService : IDisposable
    {
        Task<bool> DoesTableExistAsync(string name);
    }
}