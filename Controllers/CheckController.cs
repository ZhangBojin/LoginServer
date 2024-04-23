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

        //[HttpGet]
        //public IActionResult Error()
        //{
        //    var client = new ConsulClient(c =>
        //    {
        //        c.Address = new Uri("http://localhost:8500");
        //        c.Datacenter = "YuWen";
        //    });
        //    var result = client.Agent.Services().Result.Response;
        //    return Ok(result);
        //}
    }
}
