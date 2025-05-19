using RentApartments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApartments.Domain.Exceptions
{
    public class LandlordDoesNotOwnApartmentException : InvalidOperationException
    {
        public Landlord Landlord { get; }
        public Apartment Apartment { get; }

        public LandlordDoesNotOwnApartmentException(Landlord landlord, Apartment apartment)
            : base($"Landlord with ID '{landlord.Id}' does not own the apartment with ID '{apartment.Id}'.")
        {
            Landlord = landlord;
            Apartment = apartment;
        }
    }
}
