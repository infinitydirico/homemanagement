using HomeManagement.Api.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Filters
{
    public class AuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        public virtual async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var header = context.HttpContext?.Request?.Headers?["Authorization"].FirstOrDefault();

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

            context.HttpContext.User = new GenericPrincipal(
                new ClaimsIdentity(token.Claims),
                Array.Empty<string>());


            await next();
        }
    }
}