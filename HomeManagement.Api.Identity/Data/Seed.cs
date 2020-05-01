using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace HomeManagement.Api.Identity.Data
{
    public static class Seed
    {
        public static void SeedRoles(this WebIdentityDbContext context)
        {
            if (context.Roles.Count() > 0) return;

            context.Roles.AddRange(
                new IdentityRole
                {
                    Name = "Administrator"
                },
                new IdentityRole
                {
                    Name = "RegularUser"
                }
            );

            context.SaveChanges();
        }
    }
}
