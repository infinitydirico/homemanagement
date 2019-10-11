using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

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

        public static string GetDatabaseFilePath(this DbContextOptionsBuilder options)
        {
            var dataSource = $"Data Source = {CreateDatabaseFilePath(null)}";
            return dataSource;
        }

        public static string CreateDatabaseFilePath(this IApplicationBuilder applicationBuilder)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var finalPath = $@"{path}\HomeManagement";

            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }

            return $@"{finalPath}\HomeManagement.db";
        }
    }
}
