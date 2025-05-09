using System;
using System.Threading;
using System.Threading.Tasks;
using SettlementBookingSystem.Domain;
using SettlementBookingSystem.Infrastructure.DbContexts;

namespace SettlementBookingSystem.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookingDbContext _context;

        public UnitOfWork(BookingDbContext context)
        {
            _context = context;
        }

        public Task SaveChanges(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
