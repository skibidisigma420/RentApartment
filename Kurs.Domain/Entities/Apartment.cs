using RentApartments.Domain.Exceptions;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.Enums;
using System;
using RentApartments.Domain.Entities.Base;


namespace RentApartments.Domain.Entities
{
    /// <summary>
    /// Represents a rental apartment.
    /// </summary>
    public class Apartment : Entity<Guid>
    {
        #region Fields

        private readonly ICollection<RentRequest> _rentRequests = [];

        #endregion

        #region Properties

        public Title Title { get; }
        public Description Description { get; private set; }
        public Address Address { get; }
        public Money MonthlyRent { get; }

        /// <summary>
        /// Current status of the apartment.
        /// </summary>
        public ApartmentStatus Status { get; private set; }

        /// <summary>
        /// Owner (landlord) of the apartment.
        /// </summary>
        public Landlord Landlord { get; }

        /// <summary>
        /// Last rent request submitted by a tenant.
        /// </summary>
        public RentRequest? LastRequest => _rentRequests.OrderByDescending(r => r.RequestDate).FirstOrDefault();

        /// <summary>
        /// Is apartment currently available for rent.
        /// </summary>
        public bool IsAvailable => Status == ApartmentStatus.Available;

        #endregion

        #region Constructors

        protected Apartment() { }
        public Apartment(
            Guid id,
            Title title,
            Description description,
            Address address,
            Money monthlyRent,
            Landlord landlord
        ) : base(id)
        {
            Title = title ?? throw new ArgumentNullValueException(nameof(title));
            Description = description ?? throw new ArgumentNullValueException(nameof(description));
            Address = address ?? throw new ArgumentNullValueException(nameof(address));
            MonthlyRent = monthlyRent ?? throw new ArgumentNullValueException(nameof(monthlyRent));
            Landlord = landlord ?? throw new ArgumentNullValueException(nameof(landlord));
            Status = ApartmentStatus.Available;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set apartment as unavailable (e.g., rented or temporarily removed).
        /// </summary>
        public bool SetUnavailable()
        {
            if (!IsAvailable)
                return false;

            Status = ApartmentStatus.Unavailable;
            return true;
        }

        /// <summary>
        /// Marks the apartment as rented (after landlord accepts a request).
        /// </summary>
        public bool SetRented()
        {
            if (!IsAvailable || !_rentRequests.Any())
                return false;

            Status = ApartmentStatus.Rented;
            return true;
        }

        /// <summary>
        /// Handles a rent request from a tenant.
        /// </summary>
        /// <param name="request">The rent request.</param>
        /// <returns>true if request was accepted; otherwise false.</returns>
        public bool AddRentRequest(RentRequest request)
        {
            if (!ReferenceEquals(request.Apartment, this))
                throw new InvalidApartmentReferenceException(this, request);

            if (_rentRequests.Contains(request))
                throw new DuplicateRentRequestException(this, request);

            if (!IsAvailable)
                return false;

            _rentRequests.Add(request);
            return true;
        }
        public void ChangeStatus(ApartmentStatus newStatus)
        {
            // Проверяем, не является ли статус тем же, что уже установлен
            if (Status == newStatus)
                return;

            Status = newStatus;
        }
        public void UpdateDescription(string newDescription)
        {
            if (newDescription == null)
                throw new ArgumentNullException(nameof(newDescription));

            Description = new Description(newDescription); 
        }


        #endregion
    }
}
