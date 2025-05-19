using RentApartments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApartments.Domain.Exceptions
{
    public class ApartmentRentRequestMissingException(Apartment apartment)
        : InvalidOperationException($"Apartment '{apartment.Title.Value}' has no approved rent requests.")
    {
        public Apartment Apartment => apartment;
    }
}
