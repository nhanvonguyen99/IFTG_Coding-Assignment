using System;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Domain
{
    public interface IBookingRepository
    {
        Task<Booking> GetById(Guid id);
        Task Create(Booking booking);
        Task<bool> CheckTimeOverlap(TimeSpan startTime, TimeSpan endTime);
    }
}
