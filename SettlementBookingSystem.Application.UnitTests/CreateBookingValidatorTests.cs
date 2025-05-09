using System;
using FluentAssertions;
using FluentValidation;
using SettlementBookingSystem.Application.Bookings.Commands;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingValidatorTests
    {
        private readonly IValidator<CreateBookingCommand> _validator;

        public CreateBookingValidatorTests()
        {
            _validator = new CreateBookingValidator();
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenNameIsEmpty()
        {
            var command = new CreateBookingCommand { Name = string.Empty, BookingTime = "10:00" };

            var result = _validator.Validate(command);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Name));
        }

        [Theory]
        [InlineData("08", "59")]
        [InlineData("16", "01")]
        public void Validate_ShouldReturnError_WhenBookingTimeIsNotWithinBusinessHours(
            string hours,
            string minutes
        )
        {
            var command = new CreateBookingCommand
            {
                Name = "Test Booking",
                BookingTime = $"{hours}:{minutes}",
            };

            var result = _validator.Validate(command);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(command.BookingTime));
        }

        [Theory]
        [InlineData("09", "00")]
        [InlineData("16", "00")]
        public void Validate_ShouldReturnValid_WhenBookingTimeIsWithinBusinessHours(
            string hours,
            string minutes
        )
        {
            var command = new CreateBookingCommand
            {
                Name = "Test Booking",
                BookingTime = $"{hours}:{minutes}",
            };

            var result = _validator.Validate(command);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
