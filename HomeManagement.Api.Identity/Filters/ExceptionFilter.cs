using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeManagement.Api.Identity.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
        }
    }
}
