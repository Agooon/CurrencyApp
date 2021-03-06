﻿using CurrencyAppDatabase.Models.AdminPanel;
using CurrencyAppDatabase.Models.CurrencyApp;
using CurrencyAppDatabase.Models.CurrencyApp.Connections;
using CurrencyAppDatabase.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CurrencyAppDatabase.DataAccess
{
    public class CurrencyContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options) { }

        public CurrencyContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Many to many relation configuration
            CreateManyToMany(ref modelBuilder);
        }


        public DbSet<ItemTable> ItemTables { get; set; }
        public DbSet<Item> Items { get; set; }
        // For creating user accounts
        public DbSet<RegistrationCode> RegistrationCodes { get; set; }
        // Connection tables (Many to Many)
        public DbSet<UserTable> UserTables { get; set; }

        private void CreateManyToMany(ref ModelBuilder modelBuilder)
        {
            // User - Table
            modelBuilder.Entity<UserTable>()
                .HasKey(ut => new { ut.UserId, ut.TableId });
            modelBuilder.Entity<UserTable>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.ItemTables)
                .HasForeignKey(ut => ut.UserId);
            modelBuilder.Entity<UserTable>()
                .HasOne(ut => ut.Table)
                .WithMany(t => t.Users)
                .HasForeignKey(ut => ut.TableId);
        }
    }
}
