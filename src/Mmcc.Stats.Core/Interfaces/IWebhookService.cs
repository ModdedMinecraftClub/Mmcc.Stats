using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface IWebhookService
    {
        Task SendMessage(string message);
        Task SendStaffAlertMessage(string message);
        Task SendStaffAlertEmbed(Embed embed);
    }
}