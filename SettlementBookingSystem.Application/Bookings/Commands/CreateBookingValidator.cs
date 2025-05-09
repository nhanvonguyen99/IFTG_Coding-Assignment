using System;
using FluentValidation;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.BookingTime)
                .NotEmpty()
                .Matches("[0-9]{1,2}:[0-9][0-9]")
                .WithMessage("Booking time must be in HH:mm format")
                .Must(BeWithinBusinessHours)
                .WithMessage("Booking time must be between 09:00 and 16:00");
        }

        private bool BeWithinBusinessHours(string time)
        {
            if (!TimeSpan.TryParse(time, out var bookingTime))
            {
                return false;
            }

            var start = new TimeSpan(9, 0, 0);
            var end = new TimeSpan(16, 0, 0);
            return bookingTime >= start && bookingTime <= end;
        }
    }
}
