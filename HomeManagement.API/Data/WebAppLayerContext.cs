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

        public DbTransaction BeginTransaction()
        {
            lock (Locker)
            {
                if(currentTransaction == null)
                {
                    var dbContext = GetDbContext();
                    var connection = dbContext.Database.GetDbConnection();
                    
                    if (connection.State != ConnectionState.Open)
                    {
                        dbContext.Database.OpenConnection();
                    }

                    currentTransaction = connection.BeginTransaction();
                }
                
                return currentTransaction;
            }
        }

        public DbTransaction GetCurrentTransaction() => currentTransaction;

        public DbContext GetDbContext()
        {
            if(dbContext == null)
            {
                var scope = serviceScopeFactory.CreateScope();
                dbContext = scope.ServiceProvider.GetRequiredService<WebAppDbContext>();
            }

            if(((WebAppDbContext)dbContext).Disposed)
            {
                var scope = serviceScopeFactory.CreateScope();
                dbContext = scope.ServiceProvider.GetRequiredService<WebAppDbContext>();
            }
            
            return dbContext;
        }

        public void CommitData()
        {
            GetDbContext().SaveChanges();
        }
    }
}
