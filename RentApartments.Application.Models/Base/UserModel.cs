using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.Base
{
    public abstract record class UserModel(Guid Id, string Username)
        : IModel<Guid>;
}