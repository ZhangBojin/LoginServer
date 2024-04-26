using Consul;
using Microsoft.AspNetCore.Mvc;

namespace LoginServer.Middleware.Jwt
{
    public class JwtMiddleware(RequestDelegate next, JwtHelper jwtHelper)
    {
        private readonly RequestDelegate next = next;
        private JwtHelper jwtHelper = jwtHelper;

        public async Task Invoke(HttpContext context)
        {
            var id = context.Request.Headers.Cookie.ToString();
            var authorization = context.Request.Headers.Authorization.ToString();
            if (jwtHelper.ValidateJwtToken(authorization, id))
            {
                await next(context);
            }
            else context.Response.StatusCode = 401;
        }
    }
}
