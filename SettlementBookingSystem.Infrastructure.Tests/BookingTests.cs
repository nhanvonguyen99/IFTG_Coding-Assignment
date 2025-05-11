using System;
using FluentAssertions;
using SettlementBookingSystem.Domain;
using Xunit;

namespace SettlementBookingSystem.Infrastructure.Tests
{
    public class BookingTests
    {
        [Fact]
        public void TestBookingCreation()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(9);
            var endTime = startTime.Add(TimeSpan.FromHours(1));
            var booking = new Booking("Test Booking", startTime, endTime);

            // Act
            // Assert
            booking.Should().NotBeNull();
            booking.Name.Should().Be("Test Booking");
            booking.StartTime.Should().Be(startTime);
            booking.EndTime.Should().Be(endTime);
        }

        [Fact]
        public void TestBookingCreationWithInvalidName()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(9);
            var endTime = startTime.Add(TimeSpan.FromHours(1));

            // Act
            Action act = () => new Booking(string.Empty, startTime, endTime);

            // Assert
            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Name cannot be empty.*")
                .And.ParamName.Should()
                .Be("name");
        }

        [Fact]
        public void TestBookingCreationWithInvalidStartTime()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(8);
            var endTime = startTime.Add(TimeSpan.FromHours(1));

            // Act
            Action act = () => new Booking("Test Booking", startTime, endTime);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TestBookingCreationWithInvalidEndTime()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(9);
            var endTime = TimeSpan.FromHours(18);

            // Act
            Action act = () => new Booking("Test Booking", startTime, endTime);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TestBookingCreationWithInvalidDuration()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(9);
            var endTime = startTime.Add(TimeSpan.FromMinutes(30));

            // Act
            Action act = () => new Booking("Test Booking", startTime, endTime);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TestBookingCreationWithInvalidEndTimeOrder()
        {
            // Arrange
            var startTime = TimeSpan.FromHours(10);
            var endTime = startTime.Add(TimeSpan.FromHours(-1));

            // Act
            Action act = () => new Booking("Test Booking", startTime, endTime);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
