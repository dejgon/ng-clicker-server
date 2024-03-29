﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClickerAPI.Services;
using ClickerAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace ClickerAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("My Policy")]
    public class StatsController : Controller
    {
        private readonly StatisticsService _statsService;

        public StatsController(StatisticsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            List<Statistics> stats = _statsService.Get();
            return Ok(new { values = stats });
        }
            

        [Route("{id:length(24)}")]
        [HttpGet]
        public ActionResult<Statistics> Get(string id)
        {
            return _statsService.Get(id);
        }

        [Route("{username}")]
        [HttpGet]
        public ActionResult<Statistics> GetByUsername(string username)
        {
            return _statsService.GetByUsername(username);
        }


        public class Username
        {
            public string username { get; set; }

            public override string ToString()
            {
                return username;
            }
        }

        [HttpPost]
        [EnableCors("My Policy")]
        public ActionResult<Statistics> Create([FromBody] Username username )
        {
            Statistics stats;
            if (_statsService.GetByUsername(username.ToString()) == null)
            {
                stats = new Statistics(username.ToString());
                _statsService.Create(stats);
            }
            else
            {
                return StatusCode(409, "User exists in database!");
            }

            return CreatedAtRoute("GetUser", new { id = stats.Id.ToString() }, stats);
        }

        [Authorize]
        [Route("{id:length(24)}")]
        [HttpPut]
        public ActionResult Update(string id, [FromBody] Statistics statsIn)
        {
            var stats = _statsService.Get(id);

            if (stats == null)
            {
                return NotFound();
            }

            _statsService.Update(id, statsIn);

            return Ok(new { Message ="przeszlo"});
        }
    }
}