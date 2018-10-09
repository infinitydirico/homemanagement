using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.App.Data
{
    public class MobileAppLayerContext : IPlatformContext
    {
        MobileAppDbContext dbContext;

        public MobileAppLayerContext()
        {
            dbContext = new MobileAppDbContext();
        }

        public DbContext GetDbContext() => dbContext;
    }
}
