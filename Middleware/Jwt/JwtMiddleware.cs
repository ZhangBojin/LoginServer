using Consul;
using Microsoft.AspNetCore.Mvc;

namespace LoginServer.Middleware.Jwt
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;
        private JwtHelper jwtHelper;

        public JwtMiddleware(RequestDelegate next, JwtHelper jwtHelper)
        {
            this.next = next;
            this.jwtHelper = jwtHelper;
        }

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
