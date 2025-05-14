using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;

public interface IRentalAgreementRepository : IRepository<RentalAgreement, Guid>
{
    // Переименовываем методы, чтобы избежать скрытия
    Task<RentalAgreement?> GetRentalAgreementByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<RentalAgreement?> AddRentalAgreementAsync(RentalAgreement agreement, CancellationToken cancellationToken);
    Task UpdateRentalAgreementAsync(RentalAgreement agreement, CancellationToken cancellationToken);

    Task<IEnumerable<RentalAgreement>> GetActiveByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
    Task<IEnumerable<RentalAgreement>> GetActiveByLandlordIdAsync(Guid landlordId, CancellationToken cancellationToken);
    Task<IEnumerable<RentalAgreement>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
}
