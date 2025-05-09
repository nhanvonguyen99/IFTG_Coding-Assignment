using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SettlementBookingSystem.Tests
{
    public class BookingControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BookingControllerTests()
        {
            _factory = new WebApplicationFactory<Program>();
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
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
