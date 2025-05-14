using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.Landlord
{
    public record class LandlordModel(
        Guid Id,
        string Username,
        IReadOnlyCollection<Guid> ActiveApartmentIds
    ) : IModel<Guid>;
}
