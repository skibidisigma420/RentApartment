using RentApartments.Application.Models.Base;

namespace RentApartments.Application.Models.Base
{
    public abstract record class UserCreateModel(Guid Id, string Username)
        : ICreateModel;
}