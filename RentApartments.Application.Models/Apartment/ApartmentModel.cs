

using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.Apartment
{
    public record class ApartmentModel(
        Guid Id,
        string Title,
        string Description,
        string Address,
        decimal MonthlyRent,
        Guid LandlordId,
        bool IsAvailable
    ) : IModel<Guid>;
}
