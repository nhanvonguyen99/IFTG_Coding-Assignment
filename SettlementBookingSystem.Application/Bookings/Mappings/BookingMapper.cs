using System;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Domain;

namespace SettlementBookingSystem.Application.Bookings.Mappings
{
    public static class BookingMapper
    {
        public static BookingDto ToDto(this Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            return new BookingDto(booking.Id);
        }
    }
}
