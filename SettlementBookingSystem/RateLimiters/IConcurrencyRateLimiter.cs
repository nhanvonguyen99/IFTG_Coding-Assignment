using System;
using System.Threading;
using System.Threading.Tasks;

namespace SettlementBookingSystem.RateLimiters
{
    public interface IConcurrencyRateLimiter
    {
        bool TryAcquire();
        void Release();
        Task<bool> WaitForAvailabilityAsync(
            TimeSpan timeout,
            CancellationToken cancellationToken = default
        );
    }
}
