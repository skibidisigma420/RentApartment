using RentApartments.Application.Models.Apartment;
using RentApartments.Application.Services.Abstractions;
using RentApartments.Domain.Entities;
using RentApartments.Domain.Enums;
using RentApartments.Domain.Repositories.Abstractions;
using RentApartments.Domain.ValueObjects;
using AutoMapper;

namespace RentApartments.Application.Services
{
    public class ApartmentsApplicationService(
        IApartmentRepository apartmentRepository,
        ILandlordRepository landlordRepository,
        IMapper mapper)
        : IApartmentsApplicationService
    {
        public async Task<ApartmentModel?> GetApartmentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var apartment = await apartmentRepository.GetByIdAsync(id, cancellationToken);
            return apartment is null ? null : MapToModel(apartment);
        }

        public async Task<IEnumerable<ApartmentModel>> GetAvailableApartmentsAsync(CancellationToken cancellationToken)
        {
            var apartments = await apartmentRepository.GetAvailableApartmentsAsync(cancellationToken);
            return apartments.Select(MapToModel);
        }

        public async Task<ApartmentModel?> CreateApartmentAsync(CreateApartmentModel apartmentInformation, CancellationToken cancellationToken)
        {
            var landlord = await landlordRepository.GetByIdAsync(apartmentInformation.LandlordId, cancellationToken);
            if (landlord is null)
                return null;

            var apartment = new Apartment(
                Guid.NewGuid(),
                new Title(apartmentInformation.Title),
                new Description(apartmentInformation.Description),
                new Address(apartmentInformation.Address),
                new Money(apartmentInformation.MonthlyRent),
                landlord
            );

            var createdApartment = await apartmentRepository.AddAsync(apartment, cancellationToken);
            return createdApartment is null ? null : MapToModel(createdApartment);
        }

        public async Task<bool> UpdateApartmentDescriptionAsync(Guid apartmentId, string newDescription, CancellationToken cancellationToken)
        {
            var apartment = await apartmentRepository.GetByIdAsync(apartmentId, cancellationToken);
            if (apartment is null)
                return false;

            apartment.UpdateDescription(newDescription);
            return await apartmentRepository.UpdateAsync(apartment, cancellationToken);
        }

        public async Task<bool> ChangeApartmentStatusAsync(Guid apartmentId, ApartmentStatus newStatus, CancellationToken cancellationToken)
        {
            var apartment = await apartmentRepository.GetByIdAsync(apartmentId, cancellationToken);
            if (apartment is null)
                return false;

            apartment.ChangeStatus(newStatus);
            return await apartmentRepository.UpdateAsync(apartment, cancellationToken);
        }

        public async Task<bool> DeleteApartmentAsync(Guid id, CancellationToken cancellationToken)
        {
            var apartment = await apartmentRepository.GetByIdAsync(id, cancellationToken);
            if (apartment is null)
                return false;

            return await apartmentRepository.DeleteAsync(apartment, cancellationToken);
        }

        // Маппинг вручную — можно заменить AutoMapper'ом при необходимости
        private static ApartmentModel MapToModel(Apartment apartment)
        {
            return new ApartmentModel(
                apartment.Id,
                apartment.Title.Value,
                apartment.Description.Value,
                apartment.Address.Value,
                apartment.MonthlyRent.Value,
                apartment.Landlord.Id,
                apartment.IsAvailable
            );
        }
    }
}
