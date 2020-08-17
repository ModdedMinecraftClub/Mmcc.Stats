using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core.Data.Models;

namespace Mmcc.Stats.Features.Tps
{
    [ApiController]
    [Route("/api/tps")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TpsStat>>> Get()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TpsStat>> GetById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            if (res is null)
                return NotFound();
            return Ok(res);
        }
        
        [Authorize(AuthenticationSchemes = "ClientApp")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TpsStat>> Post([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id = res.Id}, res);
        }
    }
}