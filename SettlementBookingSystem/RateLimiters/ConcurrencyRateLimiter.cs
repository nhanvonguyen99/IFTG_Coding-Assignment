using System;
using System.Threading;
using System.Threading.Tasks;

namespace SettlementBookingSystem.RateLimiters
{
    public class ConcurrencyRateLimiter : IConcurrencyRateLimiter
    {
        private readonly SemaphoreSlim _semaphore;

        public ConcurrencyRateLimiter(int maxConcurrentRequests = 4)
        {
            _semaphore = new SemaphoreSlim(maxConcurrentRequests, maxConcurrentRequests);
        }

        public bool TryAcquire()
        {
            return _semaphore.Wait(0);
        }

        public void Release()
        {
            _semaphore.Release();
        }

        public async Task<bool> WaitForAvailabilityAsync(
            TimeSpan timeout,
            CancellationToken cancellationToken = default
        )
        {
            return await _semaphore.WaitAsync(timeout, cancellationToken);
        }
    }
}
