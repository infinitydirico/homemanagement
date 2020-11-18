using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace HomeManagement.API.Filters
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        string key;
        TimeSpan slidingExpiration;

        public CacheAttribute(string key, int slidingExpiration)
        {
            this.key = key;
            this.slidingExpiration = TimeSpan.FromMinutes(slidingExpiration);
        }
        

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var memoryCache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;

            var cache = memoryCache.Get(key);

            if(cache != null)
            {
                context.Result = new OkObjectResult(cache);
                return;
            }

            var executedContext = await next();

            if(executedContext.Result is OkObjectResult)
            {
                var okResult = (OkObjectResult)executedContext.Result;
                memoryCache.Set(key, okResult.Value, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = slidingExpiration
                });
            }
        }
    }
}
