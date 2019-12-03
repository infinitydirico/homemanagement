using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.AdminSite.Data
{
    public class AdminDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ConfigurationSetting> ConfigurationSettings { get; set; }

        public AdminDbContext(DbContextOptions<AdminDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<Category>().HasKey(x => x.Id);

            modelBuilder.Entity<Category>().HasOne(x => x.User).WithMany(x => x.Categories).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<ConfigurationSetting>().ToTable(nameof(ConfigurationSettings)).HasKey(x => x.Id);
        }
    }
}
