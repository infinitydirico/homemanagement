using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeManagement.Api.Identity.Data
{
    public class IdentityLayerContext : IPlatformContext
    {
        public IConfiguration configuration;

        public IdentityLayerContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public WebIdentityDbContext CreateContext()
        {
            var postgresConnection = configuration.GetSection("ConnectionStrings").GetValue<string>("Postgres");

            var optionsBuilder = new DbContextOptionsBuilder<WebIdentityDbContext>();
            optionsBuilder.UseNpgsql(postgresConnection);
            return new WebIdentityDbContext(optionsBuilder.Options);
        }

        public DbContext CreateDbContext() => CreateContext();
    }
}
