using RentApartments.Domain.Exceptions;
using RentApartments.Domain.Entities.Base;
using RentApartments.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RentApartments.Domain.Enums;

namespace RentApartments.Domain.Entities
{
    /// <summary>
    /// Represents the landlord who manages rental properties.
    /// </summary>
    public class Landlord(Guid id, Username username) : Entity<Guid>(id)
    {
        #region Fields

        /// <summary> 
        /// The landlord's rental properties (apartments).
        /// </summary>
        private readonly ICollection<Apartment> _apartments = [];

        #endregion // Fields

        #region Properties

        /// <summary> 
        /// Gets the landlord's Username. 
        /// </summary>
        public Username Username { get; private set; } = username ?? throw new ArgumentNullValueException(nameof(username));

        /// <summary>
        /// Gets the landlord's active rental properties (apartments).
        /// </summary>
        public IReadOnlyCollection<Apartment> ActiveApartments =>
            _apartments.Where(apartment => apartment.Status == ApartmentStatus.Available).ToList().AsReadOnly();

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Adds an apartment to the landlord's list of available apartments for rent.
        /// </summary>
        /// <param name="apartment">The apartment to add.</param>
        public void AddApartment(Apartment apartment)
        {
            if (_apartments.Contains(apartment))
                throw new InvalidOperationException("This apartment is already listed.");

            _apartments.Add(apartment);
        }

        /// <summary>
        /// Removes an apartment from the landlord's available apartments.
        /// </summary>
        /// <param name="apartment">The apartment to remove.</param>
        /// <returns>True if the apartment was successfully removed; otherwise, false.</returns>
        public bool RemoveApartment(Apartment apartment)
        {
            var apartmentToRemove = _apartments.FirstOrDefault(a => a.Id == apartment.Id);
            if (apartmentToRemove == null)
                return false;

            _apartments.Remove(apartmentToRemove);
            return true;
        }

        /// <summary>
        /// Changes the status of an apartment (e.g., available, unavailable).
        /// </summary>
        /// <param name="apartment">The apartment whose status is to be changed.</param>
        /// <param name="newStatus">The new status for the apartment.</param>
        public void ChangeApartmentStatus(Apartment apartment, ApartmentStatus newStatus)
        {
            var apartmentToUpdate = _apartments.FirstOrDefault(a => a.Id == apartment.Id)
                ?? throw new ApartmentDoesNotExistException(apartment);

            apartmentToUpdate.ChangeStatus(newStatus);
        }

        /// <summary>
        /// Updates the description or details of an apartment.
        /// </summary>
        /// <param name="apartment">The apartment to update.</param>
        /// <param name="newDescription">New description for the apartment.</param>
        public void UpdateApartmentDetails(Apartment apartment, string newDescription)
        {
            var apartmentToUpdate = _apartments.FirstOrDefault(a => a.Id == apartment.Id)
                ?? throw new ApartmentDoesNotExistException(apartment);

            apartmentToUpdate.UpdateDescription(newDescription);
        }

        #endregion // Methods
    }
}
