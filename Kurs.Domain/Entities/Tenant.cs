using RentApartments.Domain.Exceptions;
using RentApartments.Domain.Entities.Base;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.Enums;

namespace RentApartments.Domain.Entities
{
    /// <summary>
    /// Represents the tenant at the apartment rental service.
    /// </summary>
    public class Tenant(Guid id, Username username) : Entity<Guid>(id)
    {
        #region Fields

        /// <summary>
        /// The tenant's observable rental apartments.
        /// </summary>
        private readonly ICollection<Apartment> _observableApartments = [];

        #endregion // Fields

        #region Properties

        /// <summary> 
        /// Gets the tenant's Username. 
        /// </summary>
        public Username Username { get; private set; } = username ?? throw new ArgumentNullException(nameof(username));


        /// <summary>
        /// Gets the read-only collection of the tenant's observable apartments.
        /// </summary>
        public IReadOnlyCollection<Apartment> ObservableApartments =>
            _observableApartments.Where(apartment => apartment.Status == ApartmentStatus.Available).ToList().AsReadOnly();

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Changes the tenant's username.
        /// </summary>
        /// <param name="newUsername">New tenant's username.</param>
        public bool ChangeUsername(Username newUsername)
        {
            if (Username == newUsername) return false;
            Username = newUsername;
            return true;
        }

        /// <summary>
        /// Adds an apartment to the sequence of observable apartments.
        /// </summary>
        /// <param name="apartment">An observable apartment.</param>
        public void AddObservableApartment(Apartment apartment)
        {
            if (_observableApartments.Contains(apartment))
                return;
            _observableApartments.Add(apartment);
        }

        /// <summary>
        /// Requests to rent an apartment.
        /// </summary>
        /// <param name="apartment">Apartment to rent.</param>
        /// <returns>true if the request to rent was successfully made; otherwise false.</returns>
        public bool TryRentApartment(Apartment apartment)
        {
            if (apartment.Status != ApartmentStatus.Available)
                return false; // Can't rent unavailable apartments

            // Make the rent request
            RentRequest newRequest = new(apartment, this, apartment.Landlord);
            apartment.AddRentRequest(newRequest);
            return true;
        }

        /// <summary>
        /// Mark an apartment as a favorite (observable).
        /// </summary>
        /// <param name="apartment">Apartment to add to favorites.</param>
        public void MarkAsFavorite(Apartment apartment)
        {
            AddObservableApartment(apartment);
        }

        #endregion // Methods
    }
}
