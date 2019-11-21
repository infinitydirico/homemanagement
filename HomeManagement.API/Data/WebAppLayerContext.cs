using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeManagement.API.Data
{
    public class WebAppLayerContext : IPlatformContext
    {
        public IConfiguration configuration;

        public WebAppLayerContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Commit()
        {
        }

        private WebAppDbContext CreateContext()
        {
            var postgresConnection = configuration.GetSection("ConnectionStrings").GetValue<string>("Postgres");

            var optionsBuilder = new DbContextOptionsBuilder<WebAppDbContext>();
            optionsBuilder.UseNpgsql(postgresConnection);
            return new WebAppDbContext(optionsBuilder.Options);
        }

        public DbContext CreateDbContext() => CreateContext();
    }
}
