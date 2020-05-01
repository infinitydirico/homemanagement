using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Api.Identity.Data
{
    public class WebIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public WebIdentityDbContext(DbContextOptions options) 
            : base(options)
        {
        }
    }
}
