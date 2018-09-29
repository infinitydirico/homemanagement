using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace HomeManagement.API.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            try
            {
                this.logger.LogError(1, context.Exception, context.Exception.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(1, ex, ex.Message);
            }
        }
    }
}
