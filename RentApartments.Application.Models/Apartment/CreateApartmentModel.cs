using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.Apartment
{
    public record class CreateApartmentModel(
        string Title,
        string Description,
        string Address,
        decimal MonthlyRent,
        Guid LandlordId
    ) : ICreateModel;
}
