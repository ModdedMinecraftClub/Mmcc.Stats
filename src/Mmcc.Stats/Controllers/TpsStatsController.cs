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
    public class TpsStatsController
    {
        private readonly ILogger<TpsStatsController> _logger;
        private readonly ITpsProcessingService _service;
        
        public TpsStatsController(ILogger<TpsStatsController> logger, ITpsProcessingService service)
        {
            _logger = logger;
            _service = service;
        }
        
        [Authorize(AuthenticationSchemes = "ClientApp")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]McTpsStatDto tpsStatDto)
        {
            await _service.ProcessIncomingPostRequest(tpsStatDto);
            return new OkResult();
        }
    }
}