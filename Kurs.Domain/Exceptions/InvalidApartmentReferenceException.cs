using RentApartments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApartments.Domain.Exceptions
{
    public class InvalidApartmentReferenceException : Exception
    {
        public InvalidApartmentReferenceException(Apartment apartment, RentRequest request)
            : base($"The rent request does not belong to this apartment: {apartment.Id}") { }
    }
}
