using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface ITpsProcessingService
    {
        Task ProcessIncomingPostRequest(McTpsStatDto tpsStat);
    }
}