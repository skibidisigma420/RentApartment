
using RentApartments.Domain.Entities;

namespace RentApartments.Domain.Exceptions
{
    public class RentalAgreementAlreadyTerminatedException : InvalidOperationException
    {
        public RentalAgreement Agreement { get; }

        public RentalAgreementAlreadyTerminatedException(RentalAgreement agreement)
            : base($"The rental agreement with ID '{agreement.Id}' is already terminated.")
        {
            Agreement = agreement;
        }
    }
}
