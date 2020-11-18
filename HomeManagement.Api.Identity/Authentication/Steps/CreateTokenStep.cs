using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace HomeManagement.Api.Identity.Authentication.Steps
{
    public class CreateTokenStep : BaseStep
    {
        public CreateTokenStep(AuthenticationStrategy authenticationStrategy) : base(authenticationStrategy)
        {
        }

        public override async Task<AuthenticationResult> Validate()
        {
            var token = await CreateToken();

            var tokenResult = await authenticationStrategy.UserManager.SetAuthenticationTokenAsync(authenticationStrategy.IdentityUser, nameof(JwtSecurityToken), nameof(JwtSecurityToken), token);

            authenticationStrategy.UserModel.Token = token;
            authenticationStrategy.UserModel.ExpirationDate = DateTime.UtcNow.AddDays(1);

            return AuthenticationResult.Succeeded();
        }

        private async Task<string> CreateToken()
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler();
            var roles = await authenticationStrategy.UserManager.GetRolesAsync(authenticationStrategy.IdentityUser);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, authenticationStrategy.IdentityUser.Id),
                new Claim(JwtRegisteredClaimNames.Email, authenticationStrategy.IdentityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, authenticationStrategy.IdentityUser.Email.Substring(0, authenticationStrategy.IdentityUser.Email.IndexOf("@")))            };

            if (roles.Any())
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }            

            var token = new JwtSecurityToken
            (
                   issuer: authenticationStrategy.Configuration["Issuer"],
                   audience: authenticationStrategy.Configuration["Audience"],
                   claims: claims,
                   expires: DateTime.UtcNow.AddDays(1),
                   notBefore: DateTime.UtcNow,
                   signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationStrategy.Configuration["SigningKey"])),
                        SecurityAlgorithms.HmacSha256)
            );

            return jwtSecurityToken.WriteToken(token);
        }
    }
}