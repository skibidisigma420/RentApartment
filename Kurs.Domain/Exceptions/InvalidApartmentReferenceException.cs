using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Exceptions
{
    public class InvalidApartmentReferenceException : Exception
    {
        public InvalidApartmentReferenceException(Apartment apartment, RentRequest request)
            : base($"The rent request does not belong to this apartment: {apartment.Id}") { }
    }
}
