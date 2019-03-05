using HomeManagement.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace HomeManagement.API.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger logger;

        public ExceptionFilter()
        {

        }

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            try
            {
                if(context.Exception is Microsoft.Data.Sqlite.SqliteException)
                {
                    HandleError(context);
                }
                //this.logger.LogError(1, context.Exception, context.Exception.Message);
            }
            catch (Exception ex)
            {
                //this.logger.LogCritical(1, ex, ex.Message);
            }
        }

        private void HandleError(ExceptionContext context)
        {
            //try handle
            try
            {
                var service = context.HttpContext.RequestServices.GetService(typeof(IPlatformContext)) as IPlatformContext;
                service.Refresh();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
