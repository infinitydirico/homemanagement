using HomeManagement.App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace HomeManagement.App.Data
{
    public class MobileAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public MobileAppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HomeManagement.db");

            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            BuildUserEntity(modelBuilder);

            //BuildAccountEntity(modelBuilder);
        }

        private void BuildUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(80);
            modelBuilder.Entity<User>().Property(x => x.Password).HasMaxLength(200);
            modelBuilder.Entity<User>().Property(x => x.ChangeStamp);
            modelBuilder.Entity<User>().Property(x => x.LastApiCall);
        }

        private void BuildAccountEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(x => x.Id);
            modelBuilder.Entity<Account>().Property(x => x.Name).HasMaxLength(80);
            modelBuilder.Entity<Account>().Property(x => x.Measurable);
            modelBuilder.Entity<Account>().Property(x => x.AccountType);
            modelBuilder.Entity<Account>().Property(x => x.Balance);
            modelBuilder.Entity<Account>().Property(x => x.CurrencyId);
            modelBuilder.Entity<Account>().Property(x => x.UserId);
            modelBuilder.Entity<Account>().Property(x => x.ChangeStamp);
            modelBuilder.Entity<Account>().Property(x => x.LastApiCall);

            modelBuilder.Entity<Account>().Ignore("Charges");
            modelBuilder.Entity<Account>().Ignore("Taxes");
        }
    }
}
