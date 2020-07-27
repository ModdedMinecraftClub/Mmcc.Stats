using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Interfaces;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Controllers
{
    [ApiController]
    [Route("/api/tps-stats")]
    public class TpsStatsController
    {
        private readonly ILogger<TpsStatsController> _logger;
        private readonly ITpsService _tpsService;
        
        public TpsStatsController(ILogger<TpsStatsController> logger, ITpsService tpsService)
        {
            _logger = logger;
            _tpsService = tpsService;
        }
        
        [Authorize(AuthenticationSchemes = "ClientApp")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]TpsStat tpsStat)
        {
            await _tpsService.InsertTpsStatAsync(tpsStat);
            return new OkResult();
        }
    }
}