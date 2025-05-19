
namespace RentApartments.Domain.Exceptions
{
    public class NullApartmentException : ArgumentNullException
    {
        public NullApartmentException()
            : base("Apartment cannot be null.") { }
    }

}
