using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace HomeManagement.App.Data
{
    public class MobileAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public MobileAppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "HomeManagement.db");

            optionsBuilder.UseSqlite($"Filename={dbPath}");
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
