using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models.Settings;

namespace Mmcc.Stats.Infrastructure.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly ILogger<WebhookService> _logger;
        private readonly WebhookSettings _settings;
        private readonly DiscordWebhookClient _webhookClient;

        public WebhookService(
            ILogger<WebhookService> logger,
            WebhookSettings settings,
            DiscordWebhookClient webhookClient
            )
        {
            _logger = logger;
            _settings = settings;
            _webhookClient = webhookClient;
        }

        public async Task SendMessage(string message)
        {
            await _webhookClient.SendMessageAsync(message);
        }

        public async Task SendStaffAlertMessage(string message)
        {
            await _webhookClient.SendMessageAsync($"<{_settings.StaffRoleId}> {message}");
        }

        public async Task SendStaffAlertEmbed(Embed embed)
        {
            await _webhookClient.SendMessageAsync($"<{_settings.StaffRoleId}>", embeds: new[] {embed});
        }
    }
}