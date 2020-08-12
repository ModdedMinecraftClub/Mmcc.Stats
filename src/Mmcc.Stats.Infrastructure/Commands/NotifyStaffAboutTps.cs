using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models;
using Mmcc.Stats.Core.Data.Models.Settings;
using Mmcc.Stats.Core.Exceptions;
using TraceLd.DiscordWebhook;
using TraceLd.DiscordWebhook.Models;

namespace Mmcc.Stats.Infrastructure.Commands
{
    public class NotifyStaffAboutTps
    {
        public class Command : IRequest
        {
            public TpsStat TpsStat { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ILogger<CommandHandler> _logger;
            private readonly TpsPingSettings _settings;
            private readonly IWebhookService _webhookService;
            private readonly PollerContext _context;

            public CommandHandler(TpsPingSettings settings, IWebhookService webhookService, ILogger<CommandHandler> logger, PollerContext context)
            {
                _settings = settings;
                _webhookService = webhookService;
                _logger = logger;
                _context = context;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                // grab corresponding server details;
                var server = await _context.Servers.AsNoTracking()
                    .Where(x => x.ServerId == request.TpsStat.ServerId)
                    .SingleOrDefaultAsync(cancellationToken);
                // if there is no corresponding server, that means there is something seriously wrong with our
                // request therefore we throw;
                if (server is null)
                {
                    throw new ServerNotFoundException(
                        $"Server with ID {request.TpsStat.ServerId} not found in the database.");
                }
                
                var embed = new Embed
                {
                    Title = $"TPS of the following server has dropped below {_settings.TpsToAlertAt}",
                    Color = 15158332, // red colour;
                    Thumbnail = new UrlEntity("https://www.moddedminecraft.club/data/icon.png"),
                    Fields = new List<Field>
                    {
                        new Field("Server details", $"Server name: {server.ServerName}\nServer ID: {server.ServerId}"),
                        new Field("Average TPS over the last 10 minutes", $"{request.TpsStat.Tps:0.00}")
                    }
                };

                try
                {
                    await _webhookService.ExecuteWebhookAsync($"<@&{_settings.StaffRoleId}>", embed);
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "Exception occurred while sending an embed to Discord.");
                }
            }
        }
    }
}