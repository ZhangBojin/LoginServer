﻿using Microsoft.AspNetCore.Mvc;

namespace LoginServer.Controllers
{
    [Route("Check")]
    [ApiController]
    public class CheckController(ILogger<CheckController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult Check()
        {
            logger.LogInformation("心跳监测中...");
            return Ok();
        }
    }
}
