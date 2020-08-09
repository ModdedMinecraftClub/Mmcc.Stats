using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Exceptions;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;
using Mmcc.Stats.Infrastructure.Extensions;
using TraceLd.DiscordWebhook;
using TraceLd.DiscordWebhook.Models;

namespace Mmcc.Stats.Infrastructure.Services
{
    public class ServerTpsService : IServerTpsService
    {
        private readonly ILogger<ServerTpsService> _logger;
        private readonly ITpsService _tpsService;
        private readonly IWebhookService _webhookService;
        private readonly TpsPingSettings _settings;
        private readonly IServerService _serverService;

        public ServerTpsService(
            ILogger<ServerTpsService> logger,
            ITpsService tpsService,
            IWebhookService webhookService,
            TpsPingSettings settings,
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
            => (await Task.WhenAll((await _serverService.SelectServersAsync())
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
                })))
                .Where(data => data.TpsStats.Any());
        
        /// <summary>
        /// Handles a TPS statistic coming from a Minecraft server.
        /// </summary>
        /// <param name="tpsStatDto">A TPS statistic DTO that came from a Minecraft server</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation</returns>
        /// <exception cref="ServerNotFoundException">Throws a ServerNotFoundException if server is not found in the database</exception>
        public async Task HandleIncomingMcTps(McTpsStatDto tpsStatDto)
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
                
                var embed = new Embed
                {
                    Title = $"TPS of the following server has dropped below {_settings.TpsToAlertAt}",
                    Color = 15158332,
                    Thumbnail = new UrlEntity("https://www.moddedminecraft.club/data/icon.png"),
                    Fields = new List<Field>
                    {
                        new Field("Server details", $"Server name: {server.ServerName}\nServer ID: {server.ServerId}"),
                        new Field("Average TPS over the last 10 minutes", $"{tpsStat.Tps:0.00}")
                    }
                };

                try
                {
                    await _webhookService.ExecuteWebhookAsync($"<@&{_settings.StaffRoleId}>", embed);
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