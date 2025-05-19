
using RentApartments.Domain.Entities;
using RentApartments.Domain.ValueObjects;

namespace RentApartments.Domain.Exceptions
{
    public class InvalidApartmentMonthlyRentException(Apartment apartment, Money rent)
        : ArgumentOutOfRangeException(nameof(rent), $"Monthly rent must be greater than zero. Provided: {rent.Value}")
    {
        public Apartment Apartment => apartment;
        public Money Rent => rent;
    }
}
