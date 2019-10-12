using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace HomeManagement.API.Data
{
    public static class DbContextExtensions
    {
        private const string volumeDirectory = "homemgmtdb";

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

        public static string GetDatabaseFilePath(this DbContextOptionsBuilder options)
        {
            var dataSource = $"Data Source = {CreateDatabaseFilePath(null)}";
            return dataSource;
        }

        public static string CreateDatabaseFilePath(this IApplicationBuilder applicationBuilder)
        {
            string path = string.Empty;

            var currentDirectory = Directory.CreateDirectory(Environment.CurrentDirectory);
            var parent = currentDirectory.Parent;

            if (!parent.EnumerateDirectories().Any(x => x.FullName.Equals(volumeDirectory)))
            {
                var createdDirectory = parent.CreateSubdirectory(volumeDirectory);
                path = createdDirectory.FullName;
            }
            else
            {
                var volDir = Directory.CreateDirectory($@"{parent.FullName}{GetOsSlash()}{volumeDirectory}");
                path = volDir.FullName;
            }

            return $@"{path}{GetOsSlash()}HomeManagement.db";
        }

        public static string GetOsSlash() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "/" : @"\";
    }
}
