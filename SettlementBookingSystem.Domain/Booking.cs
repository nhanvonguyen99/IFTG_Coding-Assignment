using System;

namespace SettlementBookingSystem.Domain
{
    public class Booking
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public TimeSpan BookingTime { get; private set; }

        public Booking(string name, TimeSpan bookingTime)
        {
            Name = name;
            BookingTime = bookingTime;
        }

        protected Booking() { }
    }
}
