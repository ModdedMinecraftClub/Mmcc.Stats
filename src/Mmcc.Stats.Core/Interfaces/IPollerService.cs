using System.Threading.Tasks;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IPollerService
    {
        Task PollAsync();
    }
}