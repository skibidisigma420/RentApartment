using RentApartments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApartments.Domain.Exceptions
{
    public class ApartmentDoesNotExistException : Exception
    {
        public ApartmentDoesNotExistException(Apartment apartment)
            : base($"Apartment with ID {apartment.Id} does not exist in the landlord's list.")
        {
        }
    }

}
