using HomeManagement.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeManagement.API.Data
{
    public class WebAppLayerContext : IPlatformContext
    {
        DbContext dbContext;
        IHttpContextAccessor httpContextAccesor;
        IServiceScopeFactory serviceScopeFactory;

        public WebAppLayerContext(IServiceScopeFactory serviceScopeFactory)//IHttpContextAccessor httpContextAccesor)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            //this.httpContextAccesor = httpContextAccesor;
            //this.dbContext = dbContext;
        }

        public DbContext GetDbContext()
        {
            var scope = serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WebAppDbContext>();
            return context;
            //var context = httpContextAccesor.HttpContext.RequestServices.GetService(typeof(WebAppDbContext));
            //return (DbContext)context;
        }
    }
}
