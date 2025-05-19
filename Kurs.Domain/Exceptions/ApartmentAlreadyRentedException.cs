
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;

namespace RentApartments.Domain.Exceptions
{
    public class ApartmentAlreadyRentedException(Apartment apartment, Address address)
        : InvalidOperationException($"The apartment at address '{address.Value}' is already in active rental.")
    {
        public Apartment Apartment => apartment;
        public Address Address => address;
    }
}
