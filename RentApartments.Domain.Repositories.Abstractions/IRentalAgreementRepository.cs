using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Repositories.Abstractions
{
    public interface IRentalAgreementRepository : IRepository<RentalAgreement, Guid>
    {
        Task<IEnumerable<RentalAgreement>> GetActiveByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
        Task<IEnumerable<RentalAgreement>> GetActiveByLandlordIdAsync(Guid landlordId, CancellationToken cancellationToken);
    }
}
