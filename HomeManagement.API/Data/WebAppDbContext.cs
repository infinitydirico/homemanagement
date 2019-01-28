﻿using HomeManagement.API.Data.Entities;
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

        public DbSet<Reminder> Reminders { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Preferences> Preferences { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<StorageItem> StorageItems { get; set; }

        public bool Disposed { get; set; }

        public WebAppDbContext(DbContextOptions<WebAppDbContext> options)
            : base(options)
        {
            if (!Database.EnsureCreated())
            {
                Database.Migrate();
            }
        }

        public override void Dispose()
        {
            Disposed = true;
            base.Dispose();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Ignore(x => x.Token);

            modelBuilder.Entity<WebClient>().HasKey(x => x.Id);

            modelBuilder.Entity<Account>().HasOne(x => x.User).WithMany(x => x.Accounts).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Charge>().HasOne(x => x.Account).WithMany(x => x.Charges).HasForeignKey(x => x.AccountId);

            modelBuilder.Entity<Charge>().HasOne(x => x.Category);

            modelBuilder.Entity<UserCategory>().HasKey(x => new { x.UserId, x.CategoryId });

            modelBuilder.Entity<UserCategory>().HasOne(x => x.Category).WithMany(x => x.UserCategories).HasForeignKey(x => x.CategoryId);

            modelBuilder.Entity<UserCategory>().HasOne(x => x.User).WithMany(x => x.UserCategories).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Category>().HasKey(x => x.Id);

            modelBuilder.Entity<Reminder>().HasKey(x => x.Id);

            modelBuilder.Entity<Reminder>().HasOne(x => x.User);

            modelBuilder.Entity<Notification>().HasKey(x => x.Id);

            modelBuilder.Entity<Notification>().HasOne(x => x.Reminder);

            modelBuilder.Entity<Preferences>().HasOne(x => x.User);

            //modelBuilder.Entity<StorageItem>().has

            modelBuilder.Ignore<Share>();
            modelBuilder.Ignore<Role>();
            modelBuilder.Ignore<Tax>();
            modelBuilder.Ignore<Token>();
            modelBuilder.Ignore<UserInRole>();
        }
    }
}
