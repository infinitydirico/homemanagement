using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HomeManagement.API.Extensions;
using HomeManagement.API.Data.Entities;
using HomeManagement.API.Data.Repositories;
using HomeManagement.API.Filters;
using HomeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HomeManagement.API.Controllers.Users
{
    [Produces("application/json")]
    [Route("api/Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ITokenRepository tokenRepository;
        private readonly JwtSecurityTokenHandler jwtSecurityToken = new JwtSecurityTokenHandler();

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserModel user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string tokenValue = string.Empty;

            var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);

            if (!result.Succeeded) return Forbid();

            var appUser = await userManager.FindByEmailAsync(user.Email);

            if (tokenRepository.UserHasToken(appUser.Id))
            {
                var dbToken = tokenRepository.FirstOrDefault(x => x.UserId.Equals(appUser.Id));

                tokenValue = dbToken.Value;

                var readToken = jwtSecurityToken.ReadToken(tokenValue);

                if (!readToken.HasExpired())
                {
                    return Ok(tokenValue);
                }

                tokenValue = CreateToken(user.Email);

                dbToken.Value = tokenValue;

                tokenRepository.Update(dbToken);
            }
            else
            {
                tokenValue = CreateToken(user.Email);

                tokenRepository.Add(new IdentityUserToken<string>
                {
                    UserId = appUser.Id,
                    LoginProvider = nameof(JwtSecurityToken),
                    Name = nameof(JwtSecurityToken),
                    Value = tokenValue
                });
            }

            if (result.Succeeded) return Ok(tokenValue);
            else return BadRequest();
        }

        [Authorization]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] UserModel user)
        {
            await signInManager.SignOutAsync();

            var appUser = await userManager.FindByEmailAsync(user.Email);

            if (tokenRepository.UserHasToken(appUser.Id)) tokenRepository.Remove(appUser.Id);

            return Ok();
        }

        private string CreateToken(string email)
        {
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Issuer"],
                   audience: configuration["Audience"],
                   claims: claims,
                   expires: DateTime.UtcNow.AddDays(1),
                   notBefore: DateTime.UtcNow,
                   signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SigningKey"])),
                        SecurityAlgorithms.HmacSha256)
            );

            return jwtSecurityToken.WriteToken(token);
        }
    }
}