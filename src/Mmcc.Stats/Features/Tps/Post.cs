using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Data;
using Mmcc.Stats.Core.Data.Models;
using Mmcc.Stats.Core.Data.Models.Settings;
using Mmcc.Stats.Core.Exceptions;
using TraceLd.DiscordWebhook;
using TraceLd.DiscordWebhook.Models;

namespace Mmcc.Stats.Features.Tps
{
    public class Post
    {
        public class Command : IRequest<TpsStat>
        {
            public int ServerId { get; set; }
            public double Tps { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.ServerId)
                    .NotNull()
                    .GreaterThan(0);
                
                RuleFor(c => c.Tps)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, TpsStat>
        {
            private readonly ILogger<Handler> _logger;
            private readonly PollerContext _context;
            private readonly IMediator _mediator;
            private readonly TpsPingSettings _settings;
            private readonly IWebhookService _webhookService;

            public Handler(PollerContext context, IMediator mediator, TpsPingSettings settings, IWebhookService webhookService, ILogger<Handler> logger)
            {
                _context = context;
                _mediator = mediator;
                _settings = settings;
                _webhookService = webhookService;
                _logger = logger;
            }

            public async Task<TpsStat> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to TpsStat;
                var tpsStat = new TpsStat
                {
                    ServerId = request.ServerId,
                    Tps = request.Tps,
                    StatTime = DateTime.UtcNow
                };
                
                // notify staff if tps below set value;
                if (tpsStat.Tps < _settings.TpsToAlertAt)
                {
                    // grab corresponding server details;
                    var server = await _context.Servers
                        .AsNoTracking()
                        .Where(x => x.ServerId == tpsStat.ServerId)
                        .FirstOrDefaultAsync(cancellationToken);
                    // if there is no corresponding server, that means there is something seriously wrong with our
                    // request therefore we throw;
                    if (server is null)
                    {
                        throw new ServerNotFoundException(
                            $"Server with ID {tpsStat.ServerId} not found in the database.");
                    }
                
                    var embed = new Embed
                    {
                        Title = $"TPS of the following server has dropped below {_settings.TpsToAlertAt}",
                        Color = 15158332, // red colour;
                        Thumbnail = new UrlEntity("https://www.moddedminecraft.club/data/icon.png"),
                        Fields = new List<Field>
                        {
                            new("Server details", $"Server name: {server.ServerName}\nServer ID: {server.ServerId}"),
                            new("Average TPS over the last 10 minutes", $"{tpsStat.Tps:0.00}")
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
                
                // insert into db and return inserted entity;
                await _context.TpsStats.AddAsync(tpsStat, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                return tpsStat;
            }
        }
    }
}