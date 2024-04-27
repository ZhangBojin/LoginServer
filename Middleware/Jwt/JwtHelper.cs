using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LoginServer.Middleware.Jwt
{
    public class JwtHelper(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// 生成Jwt
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GenerateJwtToken(string uid)
        {
            var key = _configuration["Jwt:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, uid),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials:credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        /// <summary>
        /// 校验Jwt
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ValidateJwtToken(string token,string SubId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                // 验证 Token
                //tokenHandler.ValidateToken(token, validationParameters, out _);

                // 从验证后的 Token 中提取用户主体
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                // 获取 sub 声明
                var subClaim = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

                // 检查 sub 声明是否存在且与预期值匹配
                return subClaim != null && subClaim == SubId;

            }
            catch (Exception)
            {
                return false; // 如果验证失败，则返回 false
            }
        }
    }
}
