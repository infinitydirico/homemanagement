using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace HomeManagement.Api.Identity.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<ExceptionFilter>)) as ILogger<ExceptionFilter>;
            logger.LogError(1, context.Exception, context.Exception.Message);
        }
    }
}
