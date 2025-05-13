using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Repositories.Abstractions
{
    public interface ITenantRepository : IRepository<Tenant, Guid>
    {
        Task<Tenant?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}
