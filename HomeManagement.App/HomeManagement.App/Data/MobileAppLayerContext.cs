using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

        public IDbContextTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Commit()
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
