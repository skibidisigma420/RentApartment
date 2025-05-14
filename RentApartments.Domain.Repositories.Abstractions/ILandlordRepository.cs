using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;

namespace RentApartments.Domain.Repositories.Abstractions
{
    public interface ILandlordRepository : IRepository<Landlord, Guid>
    {
        Task<Landlord?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<Landlord?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Landlord?> GetByUsernameAsync(Username username, CancellationToken cancellationToken);
        Task<IEnumerable<Landlord>> GetAllAsync(CancellationToken cancellationToken);
        Task<Landlord?> AddAsync(Landlord landlord, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Landlord landlord, CancellationToken cancellationToken);

    }
}
