using HomeManagement.Api.Core;
using HomeManagement.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeManagement.API.Filters
{
    public class AdminAuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var header = context.HttpContext.GetAuthorizationHeader();

            var token = TokenFactory.Reader(header);

            var email = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            if (!TokenFactory.IsAdmin(token))
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Unauthorized };

                return;
            }

            await next();
        }
    }
}
