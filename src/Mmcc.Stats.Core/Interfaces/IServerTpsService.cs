using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IServerTpsService
    {
        Task ProcessIncomingTps(McTpsStatDto tpsStat);
    }
}