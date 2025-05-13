using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Exceptions
{
    public class DuplicateRentRequestException : Exception
    {
        public DuplicateRentRequestException(Apartment apartment, RentRequest request)
            : base($"Duplicate rent request for apartment: {apartment.Id} from tenant {request.Tenant.Id}") { }
    }
}
