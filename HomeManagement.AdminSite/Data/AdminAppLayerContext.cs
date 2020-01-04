using HomeManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeManagement.AdminSite.Data
{
    public class AdminAppLayerContext : IPlatformContext
    {
        public IConfiguration configuration;

        public AdminAppLayerContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Commit()
        {
        }

        private AdminDbContext CreateContext()
        {
            var postgresConnection = configuration.GetSection("ConnectionStrings").GetValue<string>("Postgres");

            var optionsBuilder = new DbContextOptionsBuilder<AdminDbContext>();
            optionsBuilder.UseNpgsql(postgresConnection);
            return new AdminDbContext(optionsBuilder.Options);
        }

        public DbContext CreateDbContext() => CreateContext();
    }
}
