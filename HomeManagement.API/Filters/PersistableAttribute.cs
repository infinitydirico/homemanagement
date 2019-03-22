using HomeManagement.API.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace HomeManagement.API.Filters
{
    public class PersistableAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.GetRepository().Commit();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
