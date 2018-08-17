using HomeManagement.API.Data.Entities;
using HomeManagement.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.API.Data
{
    public class WebAppDbContext : IdentityDbContext<ApplicationUser>
    {
        //public DbSet<User> Users { get; set; }
        public DbSet<WebClient> WebClients { get; set; }

        public WebAppDbContext(DbContextOptions<WebAppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ApplicationUser>().HasKey(x => x.Id);
            //modelBuilder.Entity<ApplicationUser>().Property(x => x.Email).HasMaxLength(80);

            modelBuilder.Entity<WebClient>().HasKey(x => x.Id);

            modelBuilder.Ignore<Share>();
            modelBuilder.Ignore<Charge>();
            modelBuilder.Ignore<Account>();
            modelBuilder.Ignore<Category>();
            modelBuilder.Ignore<Role>();
            modelBuilder.Ignore<Tax>();
            modelBuilder.Ignore<Token>();
            modelBuilder.Ignore<User>();
            modelBuilder.Ignore<UserCategory>();
            modelBuilder.Ignore<UserInRole>();
        }
    }
}
