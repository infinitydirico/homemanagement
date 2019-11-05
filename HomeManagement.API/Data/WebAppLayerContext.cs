using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.API.Data
{
    public class WebAppLayerContext : IPlatformContext
    {
        WebAppDbContext dbContext;

        public DbContext GetDbContext()
        {
            if (dbContext == null || dbContext.Disposed)
            {
                dbContext = CreateContext();
            }

            return dbContext;
        }

        public void Commit()
        {
            if (dbContext.ChangeTracker.HasChanges())
            {
                dbContext.SaveChanges();
            }
        }

        private WebAppDbContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebAppDbContext>();
            optionsBuilder.UseSqlite(optionsBuilder.GetDatabaseFilePath());
            return new WebAppDbContext(optionsBuilder.Options);
        }
    }
}
