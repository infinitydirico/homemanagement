using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace HomeManagement.App.Data
{
    public class MobileAppLayerContext : IPlatformContext
    {
        MobileAppDbContext dbContext;

        public MobileAppLayerContext()
        {
            dbContext = new MobileAppDbContext();
        }

        public DbContext CreateContext()
        {
            throw new NotImplementedException();
        }

        public DbContext GetDbContext() => dbContext;
    }
}
