using HomeManagement.Api.Core.Throttle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace HomeManagement.API.Filters
{
    public class ThrottleFilter : Attribute, IActionFilter
    {
        IThrottleCore throttle;
        IHttpContextAccessor accessor;

        public ThrottleFilter(IThrottleCore throttle, IHttpContextAccessor accessor)
        {
            this.throttle = throttle;
            this.accessor = accessor;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context != null)
            {
                var connectionFeature = context.HttpContext?.Features?.Get<IHttpConnectionFeature>();
                var ip = connectionFeature?.RemoteIpAddress?.ToString();
                if (string.IsNullOrEmpty(ip)) return;

                var canRequest = throttle.CanRequest(ip);

                if (!canRequest)
                {
                    context.Result = new ContentResult 
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden, 
                        Content = "You had been BANED !" 
                    };
                }
            }            
        }
    }
}
