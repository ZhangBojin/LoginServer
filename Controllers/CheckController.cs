using Microsoft.AspNetCore.Mvc;

namespace LoginServer.Controllers
{
    [Route("Check")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
