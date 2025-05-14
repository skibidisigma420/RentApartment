using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.RentalAgreement
{
    public record class CreateRentalAgreementModel(
        Guid ApartmentId,
        Guid TenantId,
        Guid LandlordId,
        decimal MonthlyRent,
        DateTime StartDate,
        DateTime CreationDate
    ) : ICreateModel;
}
