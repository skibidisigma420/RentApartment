using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.RentalAgreement
{
    public record class RentalAgreementModel(
        Guid Id,
        Guid ApartmentId,
        Guid TenantId,
        Guid LandlordId,
        decimal MonthlyRent,
        DateTime StartDate,
        DateTime? EndDate,
        bool IsActive
    ) : IModel<Guid>;
}
