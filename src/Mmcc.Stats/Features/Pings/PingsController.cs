﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Data.Dtos;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Features.Pings
{
    [ApiController]
    [Route("/api/pings")]
    public class PingsController : ControllerBase
    {
        private readonly ILogger<PingsController> _logger;
        private readonly IMediator _mediator;

        public PingsController(ILogger<PingsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Ping>>> Get()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }

        [HttpGet("server/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Ping>>> GetByServerId([FromRoute] GetByServerId.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Ping>> GetById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            if (res is null)
                return NotFound();
            return Ok(res);
        }
        
        [HttpGet("chart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ServerPlayerbaseChartData>>> GetChartData([FromQuery] GetChartData.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ServersChartData);
        }

        [HttpGet("chart/avg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ServerPlayerbaseChartData>>> GetChartDataAsAvg([FromQuery] GetChartDataAsAvg.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ServerAvgChartDataDtos);
        }

        [HttpGet("weekly/avgs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetWeeklyAvgs.Result>> GetWeeklyAvgs()
        {
            var res = await _mediator.Send(new GetWeeklyAvgs.Query());
            return Ok(res);
        }
    }
}