using RentApartments.Domain.ValueObjects.Base;
using RentApartments.Domain.ValueObjects.Exceptions;

namespace RentApartments.Domain.ValueObjects.Validators
{
    /// <summary>
    /// Defines a method that implements the validation of the address string.
    /// </summary>
    public class AddressValidator : IValidator<string>
    {
        /// <summary>
        /// Verifies the string to make sure it is not null, empty or whitespace, and has a reasonable length.
        /// </summary>
        /// <param name="value">A string containing address.</param>
        /// <exception cref="ArgumentNullOrWhiteSpaceException"></exception>
        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullOrWhiteSpaceException(nameof(value), ExceptionMessages.ADDRESS_NOT_NULL_OR_WHITE_SPACE);

            if (value.Length < 5 || value.Length > 250)
                throw new InvalidAddressLengthException(ExceptionMessages.ADDRESS_LENGTH_OUT_OF_RANGE, nameof(value), value);
        }
    }
}
