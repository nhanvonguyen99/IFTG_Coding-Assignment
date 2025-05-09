using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SettlementBookingSystem.Domain;
using SettlementBookingSystem.Infrastructure.DbContexts;

namespace SettlementBookingSystem.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _dbContext;

        public BookingRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> CheckTimeOverlap(TimeSpan startTime, TimeSpan endTime)
        {
            return _dbContext.Bookings.AnyAsync(b =>
                startTime < b.EndTime && endTime > b.StartTime
            );
        }

        public async Task Create(Booking booking)
        {
            await _dbContext.Bookings.AddAsync(booking);
        }

        public async Task<Booking> GetById(Guid id)
        {
            return await _dbContext.Bookings.FindAsync(id);
        }
    }
}
