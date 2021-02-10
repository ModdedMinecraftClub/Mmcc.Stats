using System.Threading.Tasks;

namespace Mmcc.Stats.Core.Interfaces
{
    /// <summary>
    /// Service for polling MC servers for playerbase statistics.
    /// </summary>
    public interface IPollerService
    {
        /// <summary>
        /// Polls all active MC servers for playerbase statistics.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation</returns>
        Task PollAsync();
    }
}