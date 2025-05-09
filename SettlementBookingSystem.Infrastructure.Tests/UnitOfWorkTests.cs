using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SettlementBookingSystem.Domain;
using SettlementBookingSystem.Infrastructure.DbContexts;
using SettlementBookingSystem.Infrastructure.Services;
using Xunit;

namespace SettlementBookingSystem.Infrastructure.Tests
{
    public class UnitOfWorkTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookingDbContext _context;

        public UnitOfWorkTests()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new BookingDbContext(options);
            _unitOfWork = new UnitOfWork(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task SaveChanges_ShouldCommitTransaction()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(1);
            var endTime = startTime.Add(TimeSpan.FromHours(1));
            var booking = new Booking("Test Booking", startTime, endTime);
            await _context.Bookings.AddAsync(booking);

            // Act
            await _unitOfWork.SaveChanges();

            // Assert
            var result = await _context.Bookings.FindAsync(booking.Id);
            result.Should().NotBeNull();
            result.Name.Should().Be(booking.Name);
            result.StartTime.Should().Be(booking.StartTime);
        }
    }
}
