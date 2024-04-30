using LoginServer.Middleware.Jwt;
using LoginServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Models;
using SqlSugar;

namespace LoginServer.Controllers
{
    [Route("Main/[action]")]
    [ApiController]
    public class LoginController(JwtHelper jwtHelper, ISqlSugarClient dbClient,IDistributedCache cache) : ControllerBase
    {
        [HttpPost]
        public ActionResult LoginAction(LoginInfo lgInfo)
        {
            try
            {
                var count= dbClient.Queryable<userinfo>().Where(u => u.phone == lgInfo.Account && u.pwd == lgInfo.Password).Count();
                if (count <= 0) return StatusCode(401, "未有此账号或密码错误！");
                var token = jwtHelper.GenerateJwtToken(lgInfo.Account.ToString());
                return Ok(token);

            }
            catch (Exception e)
            {
                return StatusCode(500, "服务器内部错误");
            }
        }

        [HttpGet]
        private ActionResult TestJwt()
        {
            return StatusCode(200,"成功");
        }

        [HttpGet]
        private  ActionResult TestCache()
        {
            cache.SetString("zbj", "111", new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return Ok();
        }
    }
}
