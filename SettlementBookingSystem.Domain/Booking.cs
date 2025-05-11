using System;

namespace SettlementBookingSystem.Domain
{
    public class Booking
    {
        private static readonly TimeSpan BusinessStartTime = TimeSpan.FromHours(9);
        private static readonly TimeSpan BusinessEndTime = TimeSpan.FromHours(17);
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public Booking(string name, TimeSpan startTime, TimeSpan endTime)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (
                startTime < BusinessStartTime
                || startTime > BusinessEndTime.Subtract(TimeSpan.FromHours(1))
            )
                throw new ArgumentOutOfRangeException(
                    nameof(startTime),
                    "Booking time must be between 9 and 17 hours."
                );

            if (endTime < BusinessStartTime || endTime > BusinessEndTime)
                throw new ArgumentOutOfRangeException(
                    nameof(endTime),
                    "End time must be between 9 and 17 hours."
                );

            if (endTime <= startTime)
                throw new ArgumentException("End time must be greater than start time.");

            if (endTime - startTime < TimeSpan.FromHours(1))
                throw new ArgumentException("Booking duration must be at least 1 hour.");

            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }

        protected Booking() { }
    }
}
