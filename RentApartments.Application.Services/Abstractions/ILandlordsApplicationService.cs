using RentApartments.Application.Models.Landlord;

namespace RentApartments.Application.Services.Abstractions
{
    public interface ILandlordsApplicationService
    {
        Task<LandlordModel?> GetLandlordByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<LandlordModel?> GetLandlordByUsernameAsync(string username, CancellationToken cancellationToken);

        Task<IEnumerable<LandlordModel>> GetLandlordsAsync(CancellationToken cancellationToken);

        Task<LandlordModel?> CreateLandlordAsync(CreateLandlordModel landlordInformation, CancellationToken cancellationToken);

        Task<bool> DeleteLandlordAsync(Guid id, CancellationToken cancellationToken);
    }
}
