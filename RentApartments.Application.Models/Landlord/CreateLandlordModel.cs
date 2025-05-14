using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.Landlord
{
    public record class CreateLandlordModel(
        string Username
    ) : ICreateModel;
}
