using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace SettlementBookingSystem.RateLimiters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ConcurrencyLimitAttribute : ActionFilterAttribute
    {
        private readonly int _timeoutInSeconds;

        public ConcurrencyLimitAttribute(int timeoutInSeconds = 5)
        {
            _timeoutInSeconds = timeoutInSeconds;
        }

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            var concurrencyLimiter =
                context.HttpContext.RequestServices.GetRequiredService<IConcurrencyRateLimiter>();
            bool acquired = false;

            try
            {
                // Try to acquire a permit with the specified timeout
                acquired = await concurrencyLimiter.WaitForAvailabilityAsync(
                    TimeSpan.FromSeconds(_timeoutInSeconds),
                    context.HttpContext.RequestAborted
                );

                if (!acquired)
                {
                    context.Result = new ObjectResult(
                        "Too many simultaneous settlement requests. Please try again later."
                    )
                    {
                        StatusCode = StatusCodes.Status429TooManyRequests,
                    };
                    return;
                }

                // Proceed with the action execution
                await next();
            }
            finally
            {
                // Release the permit if we acquired one
                if (acquired)
                {
                    concurrencyLimiter.Release();
                }
            }
        }
    }
}
