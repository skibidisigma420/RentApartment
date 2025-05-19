using AutoMapper;
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
        ILandlordRepository landlordRepository,
        IMapper mapper) : IRentalAgreementsApplicationService
    {
        public async Task<RentalAgreementModel?> GetAgreementByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var agreement = await rentalAgreementRepository.GetByIdAsync(id, cancellationToken);
            return agreement is null ? null : mapper.Map<RentalAgreementModel>(agreement);
        }

        public async Task<IEnumerable<RentalAgreementModel>> GetAgreementsByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            var agreements = await rentalAgreementRepository.GetByTenantIdAsync(tenantId, cancellationToken);
            return mapper.Map<IEnumerable<RentalAgreementModel>>(agreements);
        }

        public async Task<RentalAgreementModel?> CreateAgreementAsync(CreateRentalAgreementModel agreementInformation, CancellationToken cancellationToken = default)
        {
            var apartment = await apartmentRepository.GetByIdAsync(agreementInformation.ApartmentId, cancellationToken);
            var tenant = await tenantRepository.GetByIdAsync(agreementInformation.TenantId, cancellationToken);
            var landlord = await landlordRepository.GetByIdAsync(agreementInformation.LandlordId, cancellationToken);

            if (apartment is null || tenant is null || landlord is null)
                return null;

            var agreement = new RentalAgreement(
                apartment,
                tenant,
                landlord,
                new Money(agreementInformation.MonthlyRent),
                agreementInformation.StartDate,
                agreementInformation.CreationDate);

            var created = await rentalAgreementRepository.AddAsync(agreement, cancellationToken);
            return created is null ? null : mapper.Map<RentalAgreementModel>(created);
        }

        public async Task<bool> TerminateAgreementAsync(Guid agreementId, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var agreement = await rentalAgreementRepository.GetByIdAsync(agreementId, cancellationToken);
            if (agreement is null)
                return false;

            try
            {
                agreement.Terminate(endDate);
                return await rentalAgreementRepository.UpdateAsync(agreement, cancellationToken);
            }
            catch
            {
                return false;
            }
        }
    }
}
