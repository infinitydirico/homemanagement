using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HomeManagement.Api.Core
{
    public class TokenFactory
    {
        public static bool IsExpired(SecurityToken securityToken) => (securityToken.ValidTo - DateTime.UtcNow).TotalDays < default(int);

        public static JwtSecurityToken Reader(string header) => new JwtSecurityToken(header);

        public static string GetEmail(JwtSecurityToken token) => token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;

        public static string GetEmail(string header) => GetEmail(Reader(header));

        public static Claim[] CreateClaims(string email) => new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        public static string CreateToken(string email, string issuer, string audience, string signInKey)
        {
            return CreateToken(email, issuer, audience, signInKey, DateTime.UtcNow.AddDays(1));
        }

        public static string CreateToken(string email, string issuer, string audience, string signInKey, DateTime expires)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler();

            var claims = CreateClaims(email);

            var token = new JwtSecurityToken
            (
                   issuer: issuer,
                   audience: audience,
                   claims: claims,
                   expires: expires,
                   notBefore: DateTime.UtcNow,
                   signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signInKey)),
                        SecurityAlgorithms.HmacSha256)
            );

            return jwtSecurityToken.WriteToken(token);
        }
    }
}