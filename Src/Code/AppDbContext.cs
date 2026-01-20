using WaterPaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace WaterPaymentSystem.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin123",
                    Email = "admin@vodokanal.ru",
                    Role = "Admin",
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}
