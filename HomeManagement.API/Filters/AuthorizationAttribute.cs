using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HomeManagement.API.Extensions;
using System.IdentityModel.Tokens.Jwt;
using HomeManagement.API.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using HomeManagement.API.Data.Entities;

namespace HomeManagement.API.Filters
{
    public class AuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        private JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var header = context.HttpContext.GetAuthorizationHeader();

            if (string.IsNullOrEmpty(header))
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Forbidden, Content = "Header not present" };

                return;
            }

            var token = header.GetJwtSecurityToken();

            if (!token.IsValid())
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Forbidden, Content = "Token has expired" };

                return;
            }

            var tokenRepository = context.HttpContext.RequestServices.GetService(typeof(ITokenRepository)) as ITokenRepository;
            var userManager = context.HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            var email = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            var user = await userManager.FindByEmailAsync(email.Value);

            if(user == null)
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.NotFound, Content = "The user does not exists" };

                return;
            }

            var dbToken = tokenRepository.FirstOrDefault(x => x.UserId.Equals(user.Id));

            if(dbToken.Value != header)
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Forbidden, Content = "Token is invalid" };

                return;
            }

            await next();
        }
    }
}
