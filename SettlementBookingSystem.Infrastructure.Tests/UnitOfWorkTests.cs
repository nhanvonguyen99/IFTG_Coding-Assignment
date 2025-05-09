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
        }

        [Fact]
        public async Task SaveChanges_ShouldCommitTransaction()
        {
            // Arrange
            var booking = new Booking("Test Booking", TimeSpan.FromHours(1));
            await _context.Bookings.AddAsync(booking);

            // Act
            await _unitOfWork.SaveChanges();

            // Assert
            var result = await _context.Bookings.FindAsync(booking.Id);
            result.Should().NotBeNull();
            result.Name.Should().Be(booking.Name);
            result.BookingTime.Should().Be(booking.BookingTime);
        }
    }
}
