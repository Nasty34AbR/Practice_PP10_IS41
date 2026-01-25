using Microsoft.EntityFrameworkCore;

namespace VodokanalWeb.Models
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<Accrual> Accruals { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Subscriber> Subscribers { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Meter> Meters { get; set; } = null!;
        public DbSet<User> Users { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; } = null!;
        public DbSet<SubscriberType> SubscriberTypes { get; set; } = null!; 
        public DbSet<SystemLog> SystemLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Accrual>()
                .HasOne(a => a.Subscriber)
                .WithMany()
                .HasForeignKey(a => a.SubscriberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Accrual>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Subscriber)
                .WithMany()
                .HasForeignKey(p => p.SubscriberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Accrual)
                .WithMany()
                .HasForeignKey(p => p.AccrualId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
