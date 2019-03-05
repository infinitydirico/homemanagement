using HomeManagement.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace HomeManagement.API.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            try
            {
                if (context.Exception is Microsoft.Data.Sqlite.SqliteException)
                {
                    throw context.Exception;
                }
                var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<ExceptionFilter>)) as ILogger<ExceptionFilter>;
                logger.LogError(1, context.Exception, context.Exception.Message);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
