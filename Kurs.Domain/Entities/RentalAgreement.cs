using RentApartments.Domain.Exceptions;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.Entities.Base;

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

        #region Constructors

        /// <summary>
        /// Protected constructor for ORM (EF Core).
        /// </summary>
        protected RentalAgreement() { }

        /// <summary>
        /// Public constructor — auto-generates ID.
        /// </summary>
        /// <param name="apartment">Apartment being rented.</param>
        /// <param name="tenant">Tenant involved.</param>
        /// <param name="landlord">Landlord offering the apartment.</param>
        /// <param name="monthlyRent">Monthly rent amount.</param>
        /// <param name="startDate">Start date of the rental.</param>
        /// <param name="creationDate">Date of agreement creation.</param>
        public RentalAgreement(
            Apartment apartment,
            Tenant tenant,
            Landlord landlord,
            Money monthlyRent,
            DateTime startDate,
            DateTime creationDate)
            : this(Guid.NewGuid(), apartment, tenant, landlord, monthlyRent, startDate, creationDate)
        {
        }

        /// <summary>
        /// Main constructor — includes explicit ID.
        /// </summary>
        /// <param name="id">Rental agreement ID.</param>
        /// <param name="apartment">Apartment being rented.</param>
        /// <param name="tenant">Tenant involved.</param>
        /// <param name="landlord">Landlord offering the apartment.</param>
        /// <param name="monthlyRent">Monthly rent amount.</param>
        /// <param name="startDate">Start date of the rental.</param>
        /// <param name="creationDate">Date of agreement creation.</param>
        protected RentalAgreement(
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

        // Метод для расторжения договора аренды
        /// <summary>
        /// Terminates the rental agreement.
        /// </summary>
        /// <param name="endDate">The date on which the rental ends.</param>
        /// <exception cref="InvalidOperationException">If the agreement is already terminated or the end date is invalid.</exception>
        public void Terminate(DateTime endDate)
        {
            if (!IsActive)
                throw new RentalAgreementAlreadyTerminatedException(this);

            if (endDate < StartDate)
                throw new RentalAgreementInvalidEndDateException(this, StartDate, endDate);

            EndDate = endDate;
        }


        #endregion
    }
}
