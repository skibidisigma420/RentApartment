using RentApartments.Application.Models.RentalAgreement;

namespace RentApartments.Application.Services.Abstractions
{
    public interface IRentalAgreementsApplicationService
    {
        Task<RentalAgreementModel?> GetAgreementByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<RentalAgreementModel>> GetAgreementsByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);

        Task<RentalAgreementModel?> CreateAgreementAsync(CreateRentalAgreementModel agreementInformation, CancellationToken cancellationToken);

        Task<bool> TerminateAgreementAsync(Guid agreementId, DateTime endDate, CancellationToken cancellationToken);
    }
}
