using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using System;
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
            var userModel = memoryCache.Get(ip);

            if(userModel == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                        { "action", "Login" },
                        { "controller", "Home" }
                });
                return;
            }

            await next();
        }
    }
}
