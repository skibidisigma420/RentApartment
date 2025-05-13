using RentApartments.Domain.ValueObjects.Base;
using RentApartments.Domain.ValueObjects.Validators;

namespace RentApartments.Domain.ValueObjects
{
    /// <summary>
    /// Represents type of the apartment's address.
    /// </summary>
    /// <param name="address">The address of the apartment.</param>
    public class Address(string address)
        : ValueObject<string>(new AddressValidator(), address);
}
