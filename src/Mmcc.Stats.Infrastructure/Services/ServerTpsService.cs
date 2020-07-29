using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;
using Mmcc.Stats.Core.Models.Settings;
using Mmcc.Stats.Infrastructure.Extensions;

namespace Mmcc.Stats.Infrastructure.Services
{
    public class ServerTpsService : IServerTpsService
    {
        private readonly ILogger<ServerTpsService> _logger;
        private readonly ITpsService _tpsService;
        private readonly IWebhookService _webhookService;
        private readonly WebhookSettings _settings;
        private readonly IServerService _serverService;

        public ServerTpsService(
            ILogger<ServerTpsService> logger,
            ITpsService tpsService,
            IWebhookService webhookService,
            WebhookSettings settings,
            IServerService serverService
            )
        {
            _logger = logger;
            _tpsService = tpsService;
            _webhookService = webhookService;
            _settings = settings;
            _serverService = serverService;
        }

        public async Task<IEnumerable<ServerTpsData>> GetByDateAsync(DateTime fromDate, DateTime toDate)
            => await Task.WhenAll((await _serverService.SelectServersAsync())
                .GroupBy(x => x.ServerId)
                .Select(async y =>
                {
                    var tps = await _tpsService.SelectTpsByServerAndDateAsync(y.Key, fromDate, toDate);
                    return new ServerTpsData
                    {
                        ServerName = y.First().ServerName,
                        TpsStats = tps.Select(stat => new ServerTpsDto
                        {
                            Time = stat.StatTime,
                            Tps = stat.Tps
                        })
                    };
                }));

        public async Task ProcessIncomingTps(McTpsStatDto tpsStatDto)
        {
            // convert from dto to model;
            var tpsStat = tpsStatDto.AsTpsStat();
            
            // alert staff if TPS below set threshold;
            if (tpsStat.Tps < _settings.TpsToAlertAt)
            {
                var server = await _serverService.SelectServer(tpsStat.ServerId);
                if (server is null)
                {
                    throw new ServerNotFoundException($"Server with ID {tpsStat.ServerId} not found in the database.");
                }

                var embed = new EmbedBuilder()
                    .WithTitle($"TPS of the following server has dropped below {_settings.TpsToAlertAt}")
                    .WithColor(Color.Red)
                    .WithThumbnailUrl("https://www.moddedminecraft.club/data/icon.png")
                    .AddField("Server details", $"Server name: {server.ServerName}\nServer ID: {server.ServerId}")
                    .AddField("Average TPS over the last 10 minutes", $"{tpsStat.Tps:0.00}")
                    .WithCurrentTimestamp();

                try
                {
                    await _webhookService.SendStaffAlertEmbed(embed.Build());
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Exception occurred while sending an embed to Discord.");
                }
            }
            
            // insert stat into db;
            await _tpsService.InsertTpsStatAsync(tpsStat);
        }
    }
}