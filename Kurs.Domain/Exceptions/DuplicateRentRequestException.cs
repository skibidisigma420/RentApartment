using RentApartments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApartments.Domain.Exceptions
{
    public class DuplicateRentRequestException : Exception
    {
        public DuplicateRentRequestException(Apartment apartment, RentRequest request)
            : base($"Duplicate rent request for apartment: {apartment.Id} from tenant {request.Tenant.Id}") { }
    }
}
