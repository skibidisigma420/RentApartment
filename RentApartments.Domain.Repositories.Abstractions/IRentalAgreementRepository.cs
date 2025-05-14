using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Repositories.Abstractions
{
    public interface IRentalAgreementRepository : IRepository<RentalAgreement, Guid>
    {
        Task<IEnumerable<RentalAgreement>> GetActiveByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
        Task<IEnumerable<RentalAgreement>> GetActiveByLandlordIdAsync(Guid landlordId, CancellationToken cancellationToken);
        Task<RentalAgreement?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<RentalAgreement>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
        Task<RentalAgreement?> AddAsync(RentalAgreement agreement, CancellationToken cancellationToken);
        Task UpdateAsync(RentalAgreement agreement, CancellationToken cancellationToken);

    }
}
