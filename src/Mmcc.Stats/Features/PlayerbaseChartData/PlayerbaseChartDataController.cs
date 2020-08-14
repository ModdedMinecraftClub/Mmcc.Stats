using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Data.Dtos;

namespace Mmcc.Stats.Features.PlayerbaseChartData
{
    [ApiController]
    [Route("/api/playerbase/chart")]
    public class PlayerbaseChartDataController : ControllerBase
    {
        private readonly ILogger<PlayerbaseChartDataController> _logger;
        private readonly IMediator _mediator;
        public PlayerbaseChartDataController(
            ILogger<PlayerbaseChartDataController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ServerPlayerbaseChartData>>> Get([FromQuery] Get.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ServersChartData);
        }

        [HttpGet("avg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ServerPlayerbaseChartData>>> GetAvg([FromQuery] GetAvg.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ServerAvgChartDataDtos);
        }
    }
}