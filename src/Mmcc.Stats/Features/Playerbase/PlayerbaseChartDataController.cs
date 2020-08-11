using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Data.Dtos;
using Mmcc.Stats.Features.Playerbase.ChartData;
using Serilog;

namespace Mmcc.Stats.Features.Playerbase
{
    [ApiController]
    [Route("/api/playerbase/chart")]
    public class PlayerbaseChartDataController : ControllerBase
    {
        private readonly ILogger<PlayerbaseChartDataController> _logger;
        private readonly IMediator _mediator;
        private readonly IEnumerable<IValidator<Get.Query>> _validators;
        public PlayerbaseChartDataController(
            ILogger<PlayerbaseChartDataController> logger,
            IMediator mediator, IEnumerable<IValidator<Get.Query>> validators)
        {
            _logger = logger;
            _mediator = mediator;
            _validators = validators;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerChartDataDto>>> Get([FromQuery] Get.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ServersChartData);
        }

        [HttpGet("avg")]
        public async Task<ActionResult<IEnumerable<ServerChartDataDto>>> GetAvg(DateTime from, DateTime to, int windowSize)
        {
            var query = new GetAvg.Query{FromDateTime = from, ToDateTime = to, WindowSize = windowSize};
            var result = await _mediator.Send(query);
            return Ok(result.ServerAvgChartDataDtos);
        }
    }
}