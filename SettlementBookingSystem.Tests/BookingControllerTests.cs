using System;
using System.Net;
using System.Net.Http;
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
            var booking = new { Name = "Test Booking", BookingTime = "10:00" };

            // Simulate a conflict by creating a booking at the same time
            await client.PostAsJsonAsync("/booking", booking);

            // Act
            var response = await client.PostAsJsonAsync("/booking", booking);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task CreateBooking_ShouldReturnTooManyRequests_WhenRateLimitExceeded()
        {
            // Arrange
            var client = _factory.CreateClient();
            var booking = new { Name = "Test Booking", BookingTime = "10:00" };
            var tasks = new Task<HttpResponseMessage>[5];
            for (int i = 0; i < 5; i++)
            {
                tasks[i] = client.PostAsJsonAsync(
                    "/booking",
                    new
                    {
                        Name = $"Test Booking {i}",
                        BookingTime = TimeSpan.FromHours(10 + i).ToString(@"hh\:mm"),
                    }
                );
            }

            // Act
            var responses = await Task.WhenAll(tasks);

            // Assert
            responses.Should().Contain(r => r.StatusCode == HttpStatusCode.TooManyRequests);
            responses.Should().Contain(r => r.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateBooking_ShouldReturnOk_WhenRateLimitNotExceeded()
        {
            // Arrange
            var client = _factory.CreateClient();
            var booking = new { Name = "Test Booking", BookingTime = "10:00" };
            var tasks = new Task<HttpResponseMessage>[4];
            for (int i = 0; i < 4; i++)
            {
                tasks[i] = client.PostAsJsonAsync(
                    "/booking",
                    new
                    {
                        Name = $"Test Booking {i}",
                        BookingTime = TimeSpan.FromHours(10 + i).ToString(@"hh\:mm"),
                    }
                );
            }

            // Act
            var responses = await Task.WhenAll(tasks);

            // Assert
            responses.Should().OnlyContain(r => r.StatusCode == HttpStatusCode.OK);
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
