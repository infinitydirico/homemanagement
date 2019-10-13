using System.Linq;

namespace HomeManagement.API.Data
{
    public static class RolesInitializer
    {
        public static void SeedRoles(this WebAppDbContext context)
        {
            if (context.Roles.Count() > 0) return;

            context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole
            {
                Name = "Administrator"
            });

            context.SaveChanges();
        }
    }
}
