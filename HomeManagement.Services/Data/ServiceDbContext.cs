using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Services.Data
{
    public class ServiceDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Preferences> Preferences { get; set; }

        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<Account>().HasOne(x => x.User).WithMany(x => x.Accounts).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Category>().HasKey(x => x.Id);

            modelBuilder.Entity<Category>().HasOne(x => x.User).WithMany(x => x.Categories).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Preferences>().HasOne(x => x.User);
        }
    }
}