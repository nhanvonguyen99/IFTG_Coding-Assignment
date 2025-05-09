using System.Threading;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Domain
{
    public interface IUnitOfWork
    {
        Task SaveChanges(CancellationToken cancellationToken = default);
    }
}
