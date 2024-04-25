using LoginServer.Middleware.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginServer.Controllers
{
    [Route("Main/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private JwtHelper jwtHelper;

        public LoginController(JwtHelper jwtHelper)
        {
            this.jwtHelper = jwtHelper;
        }

        [HttpPost]
        public IActionResult gettoken(string id)
        {
            return Ok(jwtHelper.GenerateJwtToken(id));
        }

        [Authorize]
        [HttpGet]
        public IActionResult LoginAction()
        {
            return Ok();
        }
    }
}
