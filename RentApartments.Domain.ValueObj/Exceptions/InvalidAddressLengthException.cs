using System;

namespace RentApartments.Domain.ValueObjects.Exceptions
{
    public class InvalidAddressLengthException : ArgumentException
    {
        public InvalidAddressLengthException(string message, string paramName, object value)
            : base($"{message} (Parameter '{paramName}', Value: '{value}')", paramName) { }
    }
}
