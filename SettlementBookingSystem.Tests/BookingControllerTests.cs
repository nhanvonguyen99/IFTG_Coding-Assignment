using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SettlementBookingSystem.Infrastructure.DbContexts;
using Xunit;

namespace SettlementBookingSystem.Tests
{
    public class BookingControllerTests
        : IClassFixture<CustomWebApplicationFactory<Program>>,
            IDisposable
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public BookingControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateBooking_ShouldReturnOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var booking = new { Name = "Test Booking", BookingTime = "10:00" };

            // Act
            var response = await client.PostAsJsonAsync("/booking", booking);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("", "10:00")]
        [InlineData(null, "10:00")]
        [InlineData("Test Booking", "")]
        [InlineData("Test Booking", "InvalidTime")]
        [InlineData("Test Booking", null)]
        [InlineData("Test Booking", "8:00")]
        public async Task CreateBooking_ShouldReturnBadRequest_WhenBodyIsInvalid(
            string name,
            string bookingTime
        )
        {
            // Arrange
            var client = _factory.CreateClient();
            var booking = new { Name = name, BookingTime = bookingTime };

            // Act
            var response = await client.PostAsJsonAsync("/booking", booking);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateBooking_ShouldReturnConflict_WhenTimeIsNotAvailable()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Simulate a conflict by creating a booking at the same time
            await client.PostAsJsonAsync(
                "/booking",
                new { Name = "Test Booking 1", BookingTime = "10:00" }
            );
            await client.PostAsJsonAsync(
                "/booking",
                new { Name = "Test Booking 1", BookingTime = "10:10" }
            );
            await client.PostAsJsonAsync(
                "/booking",
                new { Name = "Test Booking 1", BookingTime = "10:15" }
            );
            await client.PostAsJsonAsync(
                "/booking",
                new { Name = "Test Booking 1", BookingTime = "10:20" }
            );

            // Act
            var response = await client.PostAsJsonAsync(
                "/booking",
                new { Name = "Test Booking 2", BookingTime = "10:30" }
            );

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
            dbContext.Database.EnsureDeleted();
        }
    }
}
