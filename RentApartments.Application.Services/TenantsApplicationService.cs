using RentApartments.Application.Models.Tenant;
using RentApartments.Application.Services.Abstractions;
using RentApartments.Domain.Entities;
using RentApartments.Domain.Repositories.Abstractions;
using AutoMapper;
using RentApartments.Domain.ValueObjects;

namespace RentApartments.Application.Services
{
    public class TenantsApplicationService : ITenantsApplicationService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;

        public TenantsApplicationService(ITenantRepository tenantRepository, IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
        }

        public async Task<TenantModel?> GetTenantByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id, cancellationToken);
            return tenant == null ? null : _mapper.Map<TenantModel>(tenant);
        }

        public async Task<TenantModel?> GetTenantByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.GetByUsernameAsync(username, cancellationToken);
            return tenant == null ? null : _mapper.Map<TenantModel>(tenant);
        }

        public async Task<IEnumerable<TenantModel>> GetTenantsAsync(CancellationToken cancellationToken)
        {
            var tenants = await _tenantRepository.GetAllAsync(cancellationToken);
            return tenants.Select(_mapper.Map<TenantModel>);
        }

        public async Task<TenantModel?> CreateTenantAsync(CreateTenantModel tenantInformation, CancellationToken cancellationToken)
        {
            // Создаем нового арендатора
            var tenant = new Tenant(Guid.NewGuid(), new Username(tenantInformation.Username));

            // Добавляем в репозиторий
            var createdTenant = await _tenantRepository.AddAsync(tenant, cancellationToken);
            await _tenantRepository.SaveChangesAsync(cancellationToken); // Сохраняем изменения

            return createdTenant == null ? null : _mapper.Map<TenantModel>(createdTenant);
        }

        public async Task<bool> ChangeTenantUsernameAsync(Guid tenantId, string newUsername, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.GetByIdAsync(tenantId, cancellationToken);
            if (tenant == null)
                return false;

            // Попытка изменить имя пользователя
            var success = tenant.ChangeUsername(new Username(newUsername));

            // Если имя изменилось, сохраняем изменения
            if (success)
                await _tenantRepository.SaveChangesAsync(cancellationToken);

            return success;
        }

        public async Task<bool> DeleteTenantAsync(Guid id, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id, cancellationToken);
            if (tenant == null)
                return false;

            return await _tenantRepository.DeleteAsync(tenant, cancellationToken);
        }
    }
}
