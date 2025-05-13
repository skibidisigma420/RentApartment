using RentApartments.Domain.ValueObjects.Base;
using RentApartments.Domain.ValueObjects.Validators;

namespace RentApartments.Domain.ValueObjects
{
    /// <summary>
    /// Represents type of the entity's description.
    /// </summary>
    /// <param name="description">The description of the entity.</param>
    public class Description(string description)
        : ValueObject<string>(new DescriptionValidator(), description);

}