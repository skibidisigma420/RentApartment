using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.Tenant
{
    public record class CreateTenantModel(
        string Username
    ) : ICreateModel;
}
