using HomeManagement.Api.Core;
using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Filters
{
    public class AuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var memoryCache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            var contextAccesor = context.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;

            var ip = contextAccesor.HttpContext.Connection.RemoteIpAddress.ToString();
            var userModel = memoryCache.Get(ip) as UserModel;

            if(userModel == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                        { "action", "Login" },
                        { "controller", "Home" }
                });
                return;
            }
            var token = TokenFactory.Reader(userModel.Token);

            if (!TokenFactory.IsAdmin(token))
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Unauthorized };

                return;
            }

            await next();
        }
    }
}
