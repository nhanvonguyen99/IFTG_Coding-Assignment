using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SettlementBookingSystem.Domain;
using SettlementBookingSystem.Infrastructure.DbContexts;
using SettlementBookingSystem.Infrastructure.Repositories;
using SettlementBookingSystem.Infrastructure.Services;

namespace SettlementBookingSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<BookingDbContext>(options =>
                options.UseInMemoryDatabase("BookingDb")
            );

            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
