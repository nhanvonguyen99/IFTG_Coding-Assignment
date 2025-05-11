using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SettlementBookingSystem.Domain;
using SettlementBookingSystem.Infrastructure.DbContexts;
using SettlementBookingSystem.Infrastructure.Repositories;
using Xunit;

namespace SettlementBookingSystem.Infrastructure.Tests
{
    public class BookingRepositoryTests : IDisposable
    {
        private readonly BookingDbContext _context;
        private readonly IBookingRepository _repository;

        public BookingRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(databaseName: "BookingRepositoryTests")
                .Options;

            _context = new BookingDbContext(options);
            _repository = new BookingRepository(_context);
        }

        [Fact]
        public async Task Create_ShouldAddBooking()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(9);
            var endTime = startTime.Add(TimeSpan.FromHours(1));
            var booking = new Booking("Test Booking", startTime, endTime);

            // Act
            await _repository.Create(booking);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetById(booking.Id);
            result.Should().NotBeNull();
            result.Name.Should().Be(booking.Name);
            result.StartTime.Should().Be(booking.StartTime);
        }

        [Fact]
        public async Task CheckTimeOverlap_ShouldReturnTrue_WhenTimeOverlap()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(9);
            var endTime = startTime.Add(TimeSpan.FromHours(1));
            // 9 AM to 10 AM
            var booking = new Booking("Test Booking", startTime, endTime);
            await _repository.Create(booking);
            await _context.SaveChangesAsync();

            // Act
            // 9:30 AM to 10:30 AM
            var startTimeToCheck = TimeSpan.FromHours(9.5);
            var endTimeToCheck = startTimeToCheck.Add(TimeSpan.FromHours(1));
            var isOverlapped = await _repository.CheckTimeOverlap(startTimeToCheck, endTimeToCheck);

            // Assert
            isOverlapped.Should().BeTrue();
        }

        [Fact]
        public async Task CheckTimeOverlap_ShouldReturnFalse_WhenTimeDoesNotOverlap()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(9);
            var endTime = startTime.Add(TimeSpan.FromHours(1));

            var booking = new Booking("Test Booking", startTime, endTime);
            await _repository.Create(booking);
            await _context.SaveChangesAsync();

            // Act
            // 10 AM to 11 AM
            var startTimeToCheck = TimeSpan.FromHours(10);
            var endTimeToCheck = startTimeToCheck.Add(TimeSpan.FromHours(1));
            var isOverlaped = await _repository.CheckTimeOverlap(startTimeToCheck, endTimeToCheck);

            // Assert
            isOverlaped.Should().BeFalse();
        }

        [Fact]
        public async Task GetById_ShouldReturnBooking_WhenExists()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(9);
            var endTime = startTime.Add(TimeSpan.FromHours(1));
            var booking = new Booking("Test Booking", startTime, endTime);
            await _repository.Create(booking);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetById(booking.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(booking.Name);
            result.StartTime.Should().Be(booking.StartTime);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var bookingId = Guid.NewGuid();

            // Act
            var result = await _repository.GetById(bookingId);

            // Assert
            result.Should().BeNull();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
