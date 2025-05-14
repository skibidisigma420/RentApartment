using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.RentRequest
{
    public record class RentRequestModel(
        Guid Id,
        Guid ApartmentId,
        Guid TenantId,
        Guid LandlordId,
        DateTime RequestDate,
        string Status,
        string? Message
    ) : IModel<Guid>;
}
