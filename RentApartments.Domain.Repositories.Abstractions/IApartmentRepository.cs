using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Repositories.Abstractions
{
    public interface IApartmentRepository : IRepository<Apartment, Guid>
    {
        Task<IEnumerable<Apartment>> GetAllAvailableAsync(CancellationToken cancellationToken, bool asNoTracking = false);
        Task<IEnumerable<Apartment>> GetByLandlordIdAsync(Guid landlordId, CancellationToken cancellationToken, bool asNoTracking = false);
        Task<IEnumerable<Apartment>> GetAvailableApartmentsAsync(CancellationToken cancellationToken);
        Task<Apartment?> GetApartmentByIdAsync(Guid id, CancellationToken cancellationToken);

    }
}
