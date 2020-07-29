using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Core.Models.Dto;

namespace Mmcc.Stats.Controllers
{
    [ApiController]
    [Route("/api/tps-stats")]
    public class TpsStatsController : ControllerBase
    {
        private readonly ILogger<TpsStatsController> _logger;
        private readonly IServerTpsService _service;
        
        public TpsStatsController(ILogger<TpsStatsController> logger, IServerTpsService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerTpsData>>> Get()
        {
            var now = DateTime.UtcNow;
            var then = DateTime.UtcNow.AddDays(-20);
            var result = await _service.GetByDateAsync(then, now);
            return Ok(result);
        }
        
        [Authorize(AuthenticationSchemes = "ClientApp")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]McTpsStatDto tpsStatDto)
        {
            await _service.ProcessIncomingTps(tpsStatDto);
            return Ok();
        }
    }
}