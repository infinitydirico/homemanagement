using HomeManagement.Data;
using System;

namespace HomeManagement.API.Data
{
    public class WebAppLayerContext : ILayerContext
    {
        WebAppDbContext dbContext;

        public WebAppLayerContext(WebAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(object value)
        {
            using(dbContext)
            {
                dbContext.Add(value);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
