using HomeManagement.Api.Core;
using HomeManagement.Api.Core.Extensions;
using Microsoft.AspNetCore.Http;
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
        public virtual async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
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

            var token = TokenFactory.Reader(header);

            if (TokenFactory.IsExpired(token))
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Forbidden, Content = "Token has expired" };

                return;
            }

            var email = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            context.HttpContext.User = new GenericPrincipal(new ClaimsIdentity(token.Claims), Array.Empty<string>());

            await next();
        }

        private bool IsDropboxRequest(QueryString queryString)
            => queryString.HasValue &&
            queryString.Value.Contains("code") &&
            queryString.Value.Contains("state");
    }
}
