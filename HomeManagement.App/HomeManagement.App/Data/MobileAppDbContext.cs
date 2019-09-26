using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Xamarin.Forms;

namespace HomeManagement.App.Data
{
    public class MobileAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public MobileAppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbConfig = DependencyService.Get<IDatabaseConfiguration>();

            string dbPath;
            if (dbConfig == null)
            {
                dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HomeManagement.db");
            }
            else
            {
                dbPath = dbConfig.GetDatabasePath();
            }

            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            BuildUserEntity(modelBuilder);

            BuildAccountEntity(modelBuilder);

            BuildAppSettingsEntity(modelBuilder);

            BuildChargesEntity(modelBuilder);
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
            modelBuilder.Entity<Account>().Property(x => x.NeedsUpdate);
        }

        private void BuildAppSettingsEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppSettings>().HasKey(x => x.Id);
            modelBuilder.Entity<AppSettings>().Property(x => x.Name).HasMaxLength(80);
            modelBuilder.Entity<AppSettings>().Property(x => x.Enabled);
        }

        private void BuildChargesEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasKey(x => x.Id);
            modelBuilder.Entity<Transaction>().Property(x => x.Name).HasMaxLength(80);
            modelBuilder.Entity<Transaction>().Property(x => x.ChangeStamp);
            modelBuilder.Entity<Transaction>().Property(x => x.LastApiCall);
            modelBuilder.Entity<Transaction>().Property(x => x.NeedsUpdate);
            modelBuilder.Entity<Transaction>().Property(x => x.Name);
            modelBuilder.Entity<Transaction>().Property(x => x.Price);
            modelBuilder.Entity<Transaction>().Property(x => x.Date);
            modelBuilder.Entity<Transaction>().Property(x => x.TransactionType);
            modelBuilder.Entity<Transaction>().Property(x => x.CategoryId);
            modelBuilder.Entity<Transaction>().Property(x => x.AccountId);
        }
    }
}
