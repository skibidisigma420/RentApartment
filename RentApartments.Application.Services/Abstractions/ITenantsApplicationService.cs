using RentApartments.Application.Models.Tenant;

namespace RentApartments.Application.Services.Abstractions
{
    public interface ITenantsApplicationService
    {
        Task<TenantModel?> GetTenantByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<TenantModel?> GetTenantByUsernameAsync(string username, CancellationToken cancellationToken);

        Task<IEnumerable<TenantModel>> GetTenantsAsync(CancellationToken cancellationToken);

        Task<TenantModel?> CreateTenantAsync(CreateTenantModel tenantInformation, CancellationToken cancellationToken);

        Task<bool> ChangeTenantUsernameAsync(Guid tenantId, string newUsername, CancellationToken cancellationToken);

        Task<bool> DeleteTenantAsync(Guid id, CancellationToken cancellationToken);
    }
}
