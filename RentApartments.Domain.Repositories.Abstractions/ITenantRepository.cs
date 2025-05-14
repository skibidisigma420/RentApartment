using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Repositories.Abstractions
{
    public interface ITenantRepository : IRepository<Tenant, Guid>
    {
        Task<Tenant?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken);
        Task<Tenant> AddAsync(Tenant tenant, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Tenant tenant, CancellationToken cancellationToken);
       
    }
}
