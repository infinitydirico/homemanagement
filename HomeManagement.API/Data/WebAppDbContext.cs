using HomeManagement.API.Data.Entities;
using HomeManagement.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.API.Data
{
    public class WebAppDbContext : IdentityDbContext<ApplicationUser>
    {
        //Since an User set already exists in IdentityDbContext
        public DbSet<User> UsersSet { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Charge> Charges { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<WebClient> WebClients { get; set; }

        public DbSet<UserCategory> UserCategories { get; set; }

        public bool Disposed { get; private set; }

        public WebAppDbContext(DbContextOptions<WebAppDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public override void Dispose()
        {
            Disposed = true;
            base.Dispose();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebClient>().HasKey(x => x.Id);

            modelBuilder.Entity<Account>().HasOne(x => x.User).WithMany(x => x.Accounts).HasForeignKey(x => x.UserId);
            
            modelBuilder.Entity<Charge>().HasOne(x => x.Account).WithMany(x => x.Charges).HasForeignKey(x => x.AccountId);

            modelBuilder.Entity<Charge>().HasOne(x => x.Category);

            modelBuilder.Entity<UserCategory>().HasKey(x => new { x.UserId, x.CategoryId });

            modelBuilder.Entity<UserCategory>().HasOne(x => x.Category).WithMany(x => x.UserCategories).HasForeignKey(x => x.CategoryId);

            modelBuilder.Entity<UserCategory>().HasOne(x => x.User).WithMany(x => x.UserCategories).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Category>().HasKey(x => x.Id);

            modelBuilder.Ignore<Share>();
            modelBuilder.Ignore<Role>();
            modelBuilder.Ignore<Tax>();
            modelBuilder.Ignore<Token>();
            modelBuilder.Ignore<UserInRole>();
        }
    }
}
