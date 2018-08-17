using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace HomeManagement.API.Extensions
{
    public static class JwtExtensions
    {
        public static bool IsValid(this JwtSecurityTokenHandler jwt, string token)
        {
            if (jwt.CanReadToken(token))
            {
                var jwtToken = jwt.ReadToken(token);

                return jwtToken.HasExpired() || jwtToken.IsValid();
            }

            return false;
        }

        public static bool HasExpired(this SecurityToken securityToken) => (securityToken.ValidTo - DateTime.UtcNow).TotalDays < default(int);

        public static bool IsValid(this SecurityToken securityToken) => (DateTime.UtcNow - securityToken.ValidFrom).TotalHours < 1;
    }
}
