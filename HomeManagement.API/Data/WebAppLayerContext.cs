using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.API.Data
{
    public class WebAppLayerContext : IPlatformContext
    {
        DbContext dbContext;
        private static object Locker = new object();

        public WebAppLayerContext()
        {

        }

        public DbContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebAppDbContext>();
            optionsBuilder.UseSqlite(optionsBuilder.GetDatabaseFilePath());
            dbContext = new WebAppDbContext(optionsBuilder.Options);

            return dbContext;
        }

        public DbContext GetDbContext()
        {
            lock (Locker)
            {
                if (dbContext == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<WebAppDbContext>();
                    optionsBuilder.UseSqlite(optionsBuilder.GetDatabaseFilePath());
                    dbContext = new WebAppDbContext(optionsBuilder.Options);
                }

                if (((WebAppDbContext)dbContext).Disposed)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<WebAppDbContext>();
                    optionsBuilder.UseSqlite(optionsBuilder.GetDatabaseFilePath());
                    dbContext = new WebAppDbContext(optionsBuilder.Options);
                }
            }

            return dbContext;
        }
    }
}
