﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmcc.Stats.Core;
using Mmcc.Stats.Core.Models;
using Mmcc.Stats.Infrastructure.Services;

namespace Mmcc.Stats.Controllers
{
    [ApiController]
    [Route("/api/playerbase-stats")]
    public class PlayerbaseStatsController : ControllerBase
    {
        private readonly ILogger<PlayerbaseStatsController> _logger;
        private readonly IDatabaseService _dbService;
        private readonly IServersPlayerbaseService _serversPlayerbaseService;

        public PlayerbaseStatsController(
            ILogger<PlayerbaseStatsController> logger,
            IDatabaseService dbService,
            IServersPlayerbaseService serversPlayerbaseService
            )
        {
            _logger = logger;
            _dbService = dbService;
            _serversPlayerbaseService = serversPlayerbaseService;
        }

        // GET /api/playerbase-stats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerPlayerbaseData>>> Get(DateTime from, DateTime to)
        {
            if (from > to)
            {
                return BadRequest();
            }

            if (from == to)
            {
                to = to.AddDays(1);
            }

            var data = await _serversPlayerbaseService.GetByDateAsync(from, to);

            return Ok(data);
        }
    }
}