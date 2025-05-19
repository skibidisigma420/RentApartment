using AutoMapper;
using RentApartments.Application.Models.Landlord;
using RentApartments.Application.Services.Abstractions;
using RentApartments.Domain.Entities;
using RentApartments.Domain.Repositories.Abstractions;
using RentApartments.Domain.ValueObjects;

namespace RentApartments.Application.Services
{
    public class LandlordsApplicationService(
        ILandlordRepository landlordRepository,
        IMapper mapper) : ILandlordsApplicationService
    {
        public async Task<LandlordModel?> GetLandlordByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var landlord = await landlordRepository.GetByIdAsync(id, cancellationToken);
            return landlord is null ? null : mapper.Map<LandlordModel>(landlord);
        }

        public async Task<LandlordModel?> GetLandlordByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var landlord = await landlordRepository.GetByUsernameAsync(new Username(username), cancellationToken);
            return landlord is null ? null : mapper.Map<LandlordModel>(landlord);
        }

        public async Task<IEnumerable<LandlordModel>> GetLandlordsAsync(CancellationToken cancellationToken = default)
        {
            var landlords = await landlordRepository.GetAllAsync(cancellationToken);
            return mapper.Map<IEnumerable<LandlordModel>>(landlords);
        }

        public async Task<LandlordModel?> CreateLandlordAsync(CreateLandlordModel landlordInformation, CancellationToken cancellationToken = default)
        {
            var landlord = new Landlord(Guid.NewGuid(), new Username(landlordInformation.Username));

            var created = await landlordRepository.AddAsync(landlord, cancellationToken);
            return created is null ? null : mapper.Map<LandlordModel>(created);
        }

        public async Task<bool> DeleteLandlordAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var landlord = await landlordRepository.GetByIdAsync(id, cancellationToken);
            if (landlord is null)
                return false;

            return await landlordRepository.DeleteAsync(landlord, cancellationToken);
        }
    }
}
