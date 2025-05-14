using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.RentRequest
{
    public record class CreateRentRequestModel(
        Guid ApartmentId,
        Guid TenantId,
        Guid LandlordId,
        string? Message
    ) : ICreateModel;
}
