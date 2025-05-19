using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Exceptions
{
    public class ApartmentIsNotAvailableException : InvalidOperationException
    {
        public Apartment Apartment { get; }

        public ApartmentIsNotAvailableException(Apartment apartment)
            : base($"Apartment with ID '{apartment.Id}' is not available for rent. Current status: {apartment.Status}.")
        {
            Apartment = apartment;
        }
    }
}
