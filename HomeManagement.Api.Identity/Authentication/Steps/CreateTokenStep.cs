using HomeManagement.Api.Core;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Authentication.Steps
{
    public class CreateTokenStep : BaseStep
    {
        public CreateTokenStep(AuthenticationStrategy authenticationStrategy) : base(authenticationStrategy)
        {
        }

        public override async Task<AuthenticationResult> Validate()
        {
            var roles = await authenticationStrategy.UserManager.GetRolesAsync(authenticationStrategy.IdentityUser);

            var token = roles.Any() ?
                TokenFactory.CreateToken(authenticationStrategy.IdentityUser.Email, roles, authenticationStrategy.Configuration["Issuer"], authenticationStrategy.Configuration["Audience"], authenticationStrategy.Configuration["SigningKey"], DateTime.UtcNow.AddDays(1)) :
                TokenFactory.CreateToken(authenticationStrategy.IdentityUser.Email, authenticationStrategy.Configuration["Issuer"], authenticationStrategy.Configuration["Audience"], authenticationStrategy.Configuration["SigningKey"]);

            var tokenResult = await authenticationStrategy.UserManager.SetAuthenticationTokenAsync(authenticationStrategy.IdentityUser, nameof(JwtSecurityToken), nameof(JwtSecurityToken), token);

            authenticationStrategy.UserModel.Token = token;
            authenticationStrategy.UserModel.ExpirationDate = DateTime.UtcNow.AddDays(1);

            return AuthenticationResult.Succeeded();
        }
    }
}