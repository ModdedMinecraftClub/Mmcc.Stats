using System.Threading.Tasks;

namespace Mmcc.Stats.Core
{
    public interface IPollerService
    {
        Task Poll();
    }
}