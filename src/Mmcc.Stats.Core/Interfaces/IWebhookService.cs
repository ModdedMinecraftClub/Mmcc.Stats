using System.Threading.Tasks;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IWebhookService
    {
        Task SendMessage(string message);
        Task SendStaffPingMessage(string message);
    }
}