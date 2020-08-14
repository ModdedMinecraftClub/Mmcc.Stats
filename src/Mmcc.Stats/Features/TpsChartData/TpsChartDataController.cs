using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Data.Dtos;

namespace Mmcc.Stats.Features.TpsChartData
{
    [ApiController]
    [Route("/api/tps/chart")]
    public class TpsChartDataController : ControllerBase
    {
        private readonly ILogger<TpsChartDataController> _logger;
        private readonly IMediator _mediator;

        public TpsChartDataController(ILogger<TpsChartDataController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ServerTpsChartData>>> Get([FromQuery] Get.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ServersChartData);
        }
    }
}