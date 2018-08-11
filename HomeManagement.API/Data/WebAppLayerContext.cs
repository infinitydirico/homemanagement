using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.API.Data
{
    public class WebAppLayerContext : IPlatformContext
    {
        DbContext dbContext;

        public WebAppLayerContext(WebAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbContext GetDbContext() => dbContext;
    }
}
