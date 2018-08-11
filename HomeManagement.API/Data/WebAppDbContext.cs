using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.API.Data
{
    public class WebAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public WebAppDbContext()
        {
            Database.EnsureCreated();
        }

        public WebAppDbContext(DbContextOptions<WebAppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=HomeManagement.db");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(80);
            modelBuilder.Entity<User>().Property(x => x.Password).HasMaxLength(200);

            modelBuilder.Entity<User>().Ignore("UserCategories");
            modelBuilder.Entity<User>().Ignore("UsersInRoles");
            modelBuilder.Entity<User>().Ignore("Token");
            modelBuilder.Entity<User>().Ignore("Shares");
            modelBuilder.Entity<User>().Ignore("Accounts");
        }
    }
}
