using System.Data;
using System.Data.Common;
using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeManagement.API.Data
{
    public class WebAppLayerContext : IPlatformContext
    {
        DbContext dbContext;
        IServiceScopeFactory serviceScopeFactory;
        DbTransaction currentTransaction;
        private static object Locker = new object();

        public WebAppLayerContext(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public DbContext GetDbContext()
        {
            if(dbContext == null)
            {
                var scope = serviceScopeFactory.CreateScope();
                dbContext = scope.ServiceProvider.GetRequiredService<WebAppDbContext>();
            }
            
            return dbContext;
        }
    }
}
