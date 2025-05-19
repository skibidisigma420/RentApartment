using RentApartments.Application.Models.Tenant;
using RentApartments.Application.Services.Abstractions;
using RentApartments.Domain.Entities;
using RentApartments.Domain.Repositories.Abstractions;
using RentApartments.Domain.ValueObjects;
using AutoMapper;

namespace RentApartments.Application.Services
{
    public class TenantsApplicationService(
        ITenantRepository tenantRepository,
        IMapper mapper) : ITenantsApplicationService
    {
        public async Task<TenantModel?> GetTenantByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var tenant = await tenantRepository.GetByIdAsync(id, cancellationToken);
            return tenant is null ? null : mapper.Map<TenantModel>(tenant);
        }

        public async Task<TenantModel?> GetTenantByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var tenant = await tenantRepository.GetByUsernameAsync(username, cancellationToken);
            return tenant is null ? null : mapper.Map<TenantModel>(tenant);
        }

        public async Task<IEnumerable<TenantModel>> GetTenantsAsync(CancellationToken cancellationToken = default)
        {
            var tenants = await tenantRepository.GetAllAsync(cancellationToken);
            return tenants.Select(mapper.Map<TenantModel>);
        }

        public async Task<TenantModel?> CreateTenantAsync(CreateTenantModel tenantInformation, CancellationToken cancellationToken = default)
        {
            var tenant = new Tenant(Guid.NewGuid(), new Username(tenantInformation.Username));
            var createdTenant = await tenantRepository.AddAsync(tenant, cancellationToken);
            await tenantRepository.SaveChangesAsync(cancellationToken);
            return createdTenant is null ? null : mapper.Map<TenantModel>(createdTenant);
        }

        public async Task<bool> ChangeTenantUsernameAsync(Guid tenantId, string newUsername, CancellationToken cancellationToken = default)
        {
            var tenant = await tenantRepository.GetByIdAsync(tenantId, cancellationToken);
            if (tenant is null)
                return false;

            var success = tenant.ChangeUsername(new Username(newUsername));
            if (success)
                await tenantRepository.SaveChangesAsync(cancellationToken);

            return success;
        }

        public async Task<bool> DeleteTenantAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var tenant = await tenantRepository.GetByIdAsync(id, cancellationToken);
            if (tenant is null)
                return false;

            return await tenantRepository.DeleteAsync(tenant, cancellationToken);
        }
    }
}
