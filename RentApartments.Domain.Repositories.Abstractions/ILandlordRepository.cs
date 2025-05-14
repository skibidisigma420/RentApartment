using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;

namespace RentApartments.Domain.Repositories.Abstractions
{
    public interface ILandlordRepository : IRepository<Landlord, Guid>
    {
        Task<Landlord?> GetLandlordByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Landlord?> AddLandlordAsync(Landlord landlord, CancellationToken cancellationToken);
        Task<bool> DeleteLandlordAsync(Landlord landlord, CancellationToken cancellationToken);

        Task<Landlord?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<Landlord?> GetByUsernameAsync(Username username, CancellationToken cancellationToken);
        Task<IEnumerable<Landlord>> GetAllAsync(CancellationToken cancellationToken);
    }

}
