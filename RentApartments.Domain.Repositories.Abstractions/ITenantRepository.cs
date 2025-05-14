using AuctionTrading.Domain.Repositories.Abstractions;
using RentApartments.Domain.Entities;

public interface ITenantRepository : IRepository<Tenant, Guid>
{
    Task<Tenant?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken);
}

