﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Features.Tps
{
    [ApiController]
    public class TpsController : ControllerBase
    {
        private readonly ILogger<TpsController> _logger;
        private readonly IMediator _mediator;

        public TpsController(ILogger<TpsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<TpsStat>> Get([FromQuery] Get.Query query)
        {
            var res = await _mediator.Send(query);
            if (res is null)
                return NotFound();
            return Ok(res);
        }
        
        [Authorize(AuthenticationSchemes = "ClientApp")]
        [HttpPost]
        public async Task<ActionResult<TpsStat>> Post([FromBody] Post.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id = result.Id}, result);
        }
    }
}