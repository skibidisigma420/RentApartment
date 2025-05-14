using RentApartments.Application.Models.Apartment;
using RentApartments.Domain.Enums;


namespace RentApartments.Application.Services.Abstractions
{
    public interface IApartmentsApplicationService
    {
        Task<ApartmentModel?> GetApartmentByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<ApartmentModel>> GetAvailableApartmentsAsync(CancellationToken cancellationToken);

        Task<ApartmentModel?> CreateApartmentAsync(CreateApartmentModel apartmentInformation, CancellationToken cancellationToken);

        Task<bool> UpdateApartmentDescriptionAsync(Guid apartmentId, string newDescription, CancellationToken cancellationToken);

        Task<bool> ChangeApartmentStatusAsync(Guid apartmentId, ApartmentStatus newStatus, CancellationToken cancellationToken);

        Task<bool> DeleteApartmentAsync(Guid id, CancellationToken cancellationToken);
    }
}
