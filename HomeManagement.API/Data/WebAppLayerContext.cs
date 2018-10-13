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

        public DbContext GetDbContext()
        {
            lock (Locker)
            {
                if (dbContext == null)
                {
                    var options = new DbContextOptionsBuilder<WebAppDbContext>()
                                    .UseSqlite("Data Source=HomeManagement.db")
                                    .Options;

                    dbContext = new WebAppDbContext(options);
                }

                if (((WebAppDbContext)dbContext).Disposed)
                {
                    var options = new DbContextOptionsBuilder<WebAppDbContext>()
                                    .UseSqlite("Data Source=HomeManagement.db")
                                    .Options;

                    dbContext = new WebAppDbContext(options);
                }
            }

            return dbContext;
        }
    }
}
