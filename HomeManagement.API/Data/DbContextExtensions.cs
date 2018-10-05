using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeManagement.API.Data
{
    public static class DbContextExtensions
    {
        public static void EnsureDatabaseCreated(this IApplicationBuilder applicationBuilder, bool autoMigrateDatabase = false)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices
                                        .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<WebAppDbContext>();

                var created = context.Database.EnsureCreated();

                if (autoMigrateDatabase)
                {
                    context.Database.Migrate();
                }

                context.SeedCategories();
            }
        }
    }
}
