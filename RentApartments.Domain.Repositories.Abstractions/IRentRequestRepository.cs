using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Repositories.Abstractions
{
    public interface IRentRequestRepository : IRepository<RentRequest, Guid>
    {
        Task<IEnumerable<RentRequest>> GetByApartmentIdAsync(Guid apartmentId, CancellationToken cancellationToken);
        Task<IEnumerable<RentRequest>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
        Task<IEnumerable<RentRequest>> GetByLandlordIdAsync(Guid landlordId, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
