using System;
using System.Threading.Tasks;

namespace Mmcc.Stats.Core
{
    public interface IDbTablesService : IDisposable
    {
        Task<bool> DoesTableExistAsync(string name);
    }
}