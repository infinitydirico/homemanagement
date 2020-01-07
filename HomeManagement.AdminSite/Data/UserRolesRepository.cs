using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;

namespace HomeManagement.AdminSite.Data
{
    public interface IUserRolesRepository
    {
        bool IsAdmin(string username);
    }

    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<UserRolesRepository> logger;
        string connectionString;

        public UserRolesRepository(IConfiguration configuration, 
            IMemoryCache memoryCache,
            ILogger<UserRolesRepository> logger)
        {
            connectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("IdentityConnection");
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public bool IsAdmin(string username)
        {
            try
            {
                var key = $"{nameof(UserRolesRepository.IsAdmin)}{username}";
                var isAdminEntry = memoryCache.Get(key);

                if (isAdminEntry != null)
                {
                    return (bool)isAdminEntry;
                }

                var commandText = $"SELECT COUNT(*)" +
                               "FROM \"AspNetUserRoles\" AS \"ur\"" +
                               "JOIN \"AspNetUsers\" AS \"u\"" +
                               "ON \"ur\".\"UserId\" = \"u\".\"Id\"" +
                               "JOIN \"AspNetRoles\" \"r\"" +
                               "ON \"ur\".\"RoleId\" = \"r\".\"Id\"" +
                              $"WHERE \"u\".\"UserName\" = '{username}'";

                using (var connection = new NpgsqlConnection(connectionString))
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    connection.Open();

                    var exists = (Int64)command.ExecuteScalar();
                    var isAdmin = exists > 0;

                    connection.Close();

                    memoryCache.CreateEntry(key);
                    memoryCache.Set(key, isAdmin);

                    return isAdmin;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception at UserRolesRepositry");
                return false;
            }
        }
    }
}
