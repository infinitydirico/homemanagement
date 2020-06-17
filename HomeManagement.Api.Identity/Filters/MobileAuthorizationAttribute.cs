using HomeManagement.Api.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Filters
{
    public class MobileAuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var mobileAppToken = configuration["MobileApp:Token"];

            var header = context.HttpContext.GetMobileHeader();

            if(!header.Equals(mobileAppToken))
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Forbidden, Content = "Header not present." };

                return;
            }

            await next();
        }
    }
}
