using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace HomeManagement.API.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            try
            {
                var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<ExceptionFilter>)) as ILogger<ExceptionFilter>;
                logger.LogError(1, context.Exception, context.Exception.Message);

                if (context.Exception is Business.BusinessException)
                {
                    context.Result = new ContentResult
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Content = context.Exception.Message
                    };
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
