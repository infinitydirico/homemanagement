using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;

namespace HomeManagement.App.Data
{
    public class MobileAppLayerContext : IPlatformContext
    {
        MobileAppDbContext dbContext;

        public MobileAppLayerContext()
        {
            dbContext = new MobileAppDbContext();
        }

        public DbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void CommitData()
        {
            throw new NotImplementedException();
        }

        public DbTransaction GetCurrentTransaction()
        {
            throw new NotImplementedException();
        }

        public DbContext GetDbContext() => dbContext;
    }
}
