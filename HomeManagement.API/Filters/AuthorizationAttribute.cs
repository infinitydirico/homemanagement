using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeManagement.API.Filters
{
    public class AuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var header = context.HttpContext.GetTokenHeader();

            //if (string.IsNullOrEmpty(header))
            //{
            //    context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Unauthorized, Content = "Header not present" };

            //    return Task.CompletedTask;
            //}

            //if (JwtTokenGenerator.Instance.IsValid(header))
            //{
            //    context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Unauthorized, Content = "Token has expired" };
            //}

            return Task.CompletedTask;
        }
    }
}
