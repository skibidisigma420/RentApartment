using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Repositories.Abstractions
{
    public interface ILandlordRepository : IRepository<Landlord, Guid>
    {
        Task<Landlord?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}
