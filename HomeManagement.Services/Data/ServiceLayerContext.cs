using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeManagement.Services.Data
{
    public class ServiceLayerContext : IPlatformContext
    {
        public IConfiguration configuration;

        public ServiceLayerContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private ServiceDbContext CreateContext()
        {
            var postgresConnection = configuration.GetSection("ConnectionStrings").GetValue<string>("Postgres");

            var optionsBuilder = new DbContextOptionsBuilder<ServiceDbContext>();
            optionsBuilder.UseNpgsql(postgresConnection);
            return new ServiceDbContext(optionsBuilder.Options);
        }

        public DbContext CreateDbContext() => CreateContext();
    }
}
