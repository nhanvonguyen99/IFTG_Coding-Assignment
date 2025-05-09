using Microsoft.EntityFrameworkCore;
using SettlementBookingSystem.Domain;

namespace SettlementBookingSystem.Infrastructure.DbContexts
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options)
            : base(options) { }

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>().HasKey(b => b.Id);
            modelBuilder.Entity<Booking>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Booking>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<Booking>().Property(b => b.BookingTime).IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}
