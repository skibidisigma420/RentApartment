using RentApartments.Domain.Exceptions;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.Entities;
using RentApartments.Domain.Entities.Base;
using System;

namespace RentApartments.Domain.Entities
{
    /// <summary>
    /// Represents the rental agreement between a tenant and a landlord.
    /// </summary>
    public class RentalAgreement : Entity<Guid>
    {
        #region Properties

        /// <summary>
        /// Date of agreement creation.
        /// </summary>
        public DateTime CreationDate { get; }

        /// <summary>
        /// Monthly rent agreed upon.
        /// </summary>
        public Money MonthlyRent { get; }

        /// <summary>
        /// Apartment being rented.
        /// </summary>
        public Apartment Apartment { get; }

        /// <summary>
        /// Tenant involved in the rental.
        /// </summary>
        public Tenant Tenant { get; }

        /// <summary>
        /// Landlord offering the apartment.
        /// </summary>
        public Landlord Landlord { get; }

        /// <summary>
        /// Start date of the rental.
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// End date of the rental (optional).
        /// </summary>
        public DateTime? EndDate { get; private set; }

        /// <summary>
        /// Indicates whether the agreement is currently active.
        /// </summary>
        public bool IsActive => EndDate == null || EndDate > DateTime.UtcNow;

        #endregion

        #region Constructor

        protected RentalAgreement()
        {
        }

        public RentalAgreement(
            Guid id,
            Apartment apartment,
            Tenant tenant,
            Landlord landlord,
            Money monthlyRent,
            DateTime startDate,
            DateTime creationDate)
            : base(id)
        {
            Apartment = apartment ?? throw new ArgumentNullValueException(nameof(apartment));
            Tenant = tenant ?? throw new ArgumentNullValueException(nameof(tenant));
            Landlord = landlord ?? throw new ArgumentNullValueException(nameof(landlord));
            MonthlyRent = monthlyRent ?? throw new ArgumentNullValueException(nameof(monthlyRent));
            StartDate = startDate;
            CreationDate = creationDate;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Terminates the rental agreement.
        /// </summary>
        /// <param name="endDate">The date on which the rental ends.</param>
        /// <exception cref="InvalidOperationException">If the agreement is already terminated or the end date is invalid.</exception>
        public void Terminate(DateTime endDate)
        {
            if (!IsActive)
                throw new InvalidOperationException("The rental agreement is already terminated.");

            if (endDate < StartDate)
                throw new InvalidOperationException("End date cannot be before start date.");

            EndDate = endDate;
        }

        #endregion
    }
}
