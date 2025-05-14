using RentApartments.Application.Models.RentalAgreement;
using RentApartments.Application.Services.Abstractions;
using RentApartments.Domain.Entities;
using RentApartments.Domain.Repositories.Abstractions;
using RentApartments.Domain.ValueObjects;

namespace RentApartments.Application.Services
{
    public class RentalAgreementsApplicationService(
        IRentalAgreementRepository rentalAgreementRepository,
        IApartmentRepository apartmentRepository,
        ITenantRepository tenantRepository,
        ILandlordRepository landlordRepository)
        : IRentalAgreementsApplicationService
    {
        public async Task<RentalAgreementModel?> GetAgreementByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var agreement = await rentalAgreementRepository.GetByIdAsync(id, cancellationToken);
            return agreement is null ? null : MapToModel(agreement);
        }

        public async Task<IEnumerable<RentalAgreementModel>> GetAgreementsByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            var agreements = await rentalAgreementRepository.GetByTenantIdAsync(tenantId, cancellationToken);
            return agreements.Select(MapToModel);
        }

        public async Task<RentalAgreementModel?> CreateAgreementAsync(CreateRentalAgreementModel agreementInformation, CancellationToken cancellationToken)
        {
            var apartment = await apartmentRepository.GetByIdAsync(agreementInformation.ApartmentId, cancellationToken);
            if (apartment is null)
                return null;

            var tenant = await tenantRepository.GetByIdAsync(agreementInformation.TenantId, cancellationToken);
            if (tenant is null)
                return null;

            var landlord = await landlordRepository.GetByIdAsync(agreementInformation.LandlordId, cancellationToken);
            if (landlord is null)
                return null;

            var agreement = new RentalAgreement(
                Guid.NewGuid(),
                apartment,
                tenant,
                landlord,
                new Money(agreementInformation.MonthlyRent),
                agreementInformation.StartDate,
                agreementInformation.CreationDate
            );

            var created = await rentalAgreementRepository.AddAsync(agreement, cancellationToken);
            return created is null ? null : MapToModel(created);
        }

        public async Task<bool> TerminateAgreementAsync(Guid agreementId, DateTime endDate, CancellationToken cancellationToken)
        {
            var agreement = await rentalAgreementRepository.GetByIdAsync(agreementId, cancellationToken);
            if (agreement is null)
                return false;

            try
            {
                agreement.Terminate(endDate);
                await rentalAgreementRepository.UpdateAsync(agreement, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static RentalAgreementModel MapToModel(RentalAgreement agreement)
        {
            return new RentalAgreementModel(
                agreement.Id,
                agreement.Apartment.Id,
                agreement.Tenant.Id,
                agreement.Landlord.Id,
                agreement.MonthlyRent.Value,
                agreement.StartDate,
                agreement.EndDate,
                agreement.IsActive
            );
        }
    }
}
