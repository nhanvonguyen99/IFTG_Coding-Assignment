using System;

namespace SettlementBookingSystem.Domain
{
    public class Booking
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public Booking(string name, TimeSpan startTime, TimeSpan endTime)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (startTime < TimeSpan.Zero || startTime > TimeSpan.FromHours(24))
                throw new ArgumentOutOfRangeException(
                    nameof(startTime),
                    "Booking time must be between 0 and 24 hours."
                );

            if (endTime < TimeSpan.Zero || endTime > TimeSpan.FromHours(24))
                throw new ArgumentOutOfRangeException(
                    nameof(endTime),
                    "End time must be between 0 and 24 hours."
                );

            if (endTime <= startTime)
                throw new ArgumentException("End time must be greater than start time.");

            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }

        protected Booking() { }
    }
}
