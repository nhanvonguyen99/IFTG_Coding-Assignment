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
    public class BookingRepositoryTests
    {
        private readonly BookingDbContext _context;
        private readonly IBookingRepository _repository;

        public BookingRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new BookingDbContext(options);
            _repository = new BookingRepository(_context);
        }

        [Fact]
        public async Task Create_ShouldAddBooking()
        {
            // Arrange
            var booking = new Booking("Test Booking", TimeSpan.FromHours(1));

            // Act
            await _repository.Create(booking);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetById(booking.Id);
            result.Should().NotBeNull();
            result.Name.Should().Be(booking.Name);
            result.BookingTime.Should().Be(booking.BookingTime);
        }

        [Fact]
        public async Task Exists_ShouldReturnTrue_WhenBookingTimeExists()
        {
            // Arrange
            var booking = new Booking("Test Booking", TimeSpan.FromHours(1));
            await _repository.Create(booking);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.Exists(booking.BookingTime);

            // Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task Exists_ShouldReturnFalse_WhenBookingTimeDoesNotExist()
        {
            // Arrange

            // Act
            var exists = await _repository.Exists(TimeSpan.FromHours(2));

            // Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task GetById_ShouldReturnBooking_WhenExists()
        {
            // Arrange
            var booking = new Booking("Test Booking", TimeSpan.FromHours(1));
            await _repository.Create(booking);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetById(booking.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(booking.Name);
            result.BookingTime.Should().Be(booking.BookingTime);
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
    }
}
