
namespace RentApartments.Domain.Exceptions
{
    public class ArgumentNullValueException : ArgumentNullException
    {
        public ArgumentNullValueException(string paramName)
            : base(paramName, $"Значение параметра '{paramName}' не может быть null.") { }
    }
}
