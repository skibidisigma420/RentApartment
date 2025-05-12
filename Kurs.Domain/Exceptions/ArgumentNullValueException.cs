using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApartments.Domain.Exceptions
{
    public class ArgumentNullValueException : ArgumentNullException
    {
        public ArgumentNullValueException(string paramName)
            : base(paramName, $"Значение параметра '{paramName}' не может быть null.") { }
    }
}
