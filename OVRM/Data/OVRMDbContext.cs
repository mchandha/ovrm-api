using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using OVRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OVRM.Data
{
    public class OVRMDbContext : IdentityDbContext<IdentityUser>
    {
        public OVRMDbContext(DbContextOptions<OVRMDbContext> options) : base(options)
        {

        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevent cascading from Customer

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments) // Fix: Changed 'b.Payment' to 'b.Payments' to match the expected collection type
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade); // OK to cascade from Booking
        }


    }


}
