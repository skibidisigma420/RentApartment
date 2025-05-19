
using RentApartments.Domain.Entities;
using RentApartments.Domain.Enums;

namespace RentApartments.Domain.Exceptions
{
    public class InvalidApartmentStatusTransitionException : InvalidOperationException
    {
        public ApartmentStatus CurrentStatus { get; }
        public ApartmentStatus TargetStatus { get; }
        public Apartment Apartment { get; }

        public InvalidApartmentStatusTransitionException(Apartment apartment, ApartmentStatus current, ApartmentStatus target)
            : base($"Cannot change apartment status from '{current}' to '{target}'.")
        {
            Apartment = apartment;
            CurrentStatus = current;
            TargetStatus = target;
        }
    }

}
