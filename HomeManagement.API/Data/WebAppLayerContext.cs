using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.API.Data
{
    public class WebAppLayerContext : IPlatformContext
    {
        public void Commit()
        {
        }

        private WebAppDbContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebAppDbContext>();
            optionsBuilder.UseSqlite(optionsBuilder.GetDatabaseFilePath());
            return new WebAppDbContext(optionsBuilder.Options);
        }

        public DbContext CreateDbContext() => CreateContext();
    }
}
