using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

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

                if (autoMigrateDatabase)
                {
                    var pendingMigrations = context.Database.GetPendingMigrations();
                    context.Database.Migrate();
                }
                else
                {
                    var created = context.Database.EnsureCreated();
                }

                context.SeedSettings();
                CurrencySeed.SeedCurrencies(context);
            }
        }

        public static string GetOsSlash() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "/" : @"\";
    }
}
