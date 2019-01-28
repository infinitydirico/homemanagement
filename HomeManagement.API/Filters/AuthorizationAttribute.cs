using HomeManagement.API.Data.Entities;
using HomeManagement.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HomeManagement.API.Filters
{
    public class AuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        private JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (IsDropboxRequest(context.HttpContext.Request.QueryString))
            {
                await next();
                return;
            }

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

            var userManager = context.HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            var email = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            var user = await userManager.FindByEmailAsync(email.Value);

            if (user == null)
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.NotFound, Content = "The user does not exists" };

                return;
            }

            context.HttpContext.User = new GenericPrincipal(new ClaimsIdentity(token.Claims), Array.Empty<string>());

            await next();
        }

        private bool IsDropboxRequest(QueryString queryString)
            => queryString.HasValue &&
            queryString.Value.Contains("code") &&
            queryString.Value.Contains("state");
    }
}
