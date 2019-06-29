using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace HomeManagement.API.Extensions
{
    public static class JwtExtensions
    {
        public static bool HasExpired(this SecurityToken securityToken) => (securityToken.ValidTo - DateTime.UtcNow).TotalDays < default(int);

        public static JwtSecurityToken GetJwtSecurityToken(this string header) => new JwtSecurityToken(header);
    }
}
