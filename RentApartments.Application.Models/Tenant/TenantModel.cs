using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.Tenant
{
    public record class TenantModel(
        Guid Id,
        string Username,
        IEnumerable<Guid> ObservableApartmentIds
    ) : IModel<Guid>;
}
