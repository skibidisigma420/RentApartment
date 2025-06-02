using RentApartments.Domain.Exceptions;
using RentApartments.Domain.Entities.Base;
using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.Enums;

namespace RentApartments.Domain.Entities
{
    /// <summary>
    /// класс представляющий арендатора в системе аренды квартир
    /// </summary>
    public class Tenant(Guid id, Username username) : Entity<Guid>(id)
    {
        #region Fields

        /// <summary>
        /// приватная коллекция квартир, которые арендатор отслеживает/добавил в избранное
        /// </summary>
        private readonly ICollection<Apartment> _observableApartments = [];

        #endregion // Fields

        #region Properties

        /// <summary> 
        /// имя пользователя арендатора
        /// тип Username value object
        /// </summary>
        public Username Username { get; private set; } = username ?? throw new ArgumentNullException(nameof(username));

        /// <summary>
        /// коллекция доступных для аренды квартир, которые отслеживает арендатор
        /// возвращает IReadOnlyCollection<Apartment>неизменяемую коллекцию
        /// только квартиры со статусом ApartmentStatus.Available
        /// </summary>
        public IReadOnlyCollection<Apartment> ObservableApartments =>
            _observableApartments.Where(apartment => apartment.Status == ApartmentStatus.Available).ToList().AsReadOnly();

        #endregion // Properties

        #region Methods

        /// <summary>
        /// изменяет имя пользователя арендатора
        /// </summary>
        public bool ChangeUsername(Username newUsername)
        {
            if (newUsername == null)
                throw new ArgumentNullException(nameof(newUsername));

            if (Username == newUsername) return false;

            Username = newUsername;
            return true;
        }


        /// <summary>
        /// добавляет квартиру в список отслеживаемых
        /// если квартира уже в списке ничего не делает
        /// </summary>
        public void AddObservableApartment(Apartment apartment)
        {
            if (apartment == null)
                throw new NullApartmentException();

            if (_observableApartments.Contains(apartment))
                return;

            _observableApartments.Add(apartment);
        }



        /// <summary>
        /// создать запрос на аренду квартиры
        /// </summary>
        public bool TryRentApartment(Apartment apartment)
        {
            if (apartment == null)
                throw new NullApartmentException();

            if (apartment.Status != ApartmentStatus.Available)
                throw new ApartmentIsNotAvailableException(apartment);


            var newRequest = new RentRequest(apartment, this, apartment.Landlord);

            if (!apartment.AddRentRequest(newRequest))
                return false;

            AddObservableApartment(apartment);
            return true;
        }

        /// <summary>
        /// добавляет квартиру в избранное
        /// </summary>
        public void MarkAsFavorite(Apartment apartment)
        {
            if (apartment == null)
                throw new ArgumentNullException(nameof(apartment));

            AddObservableApartment(apartment);
        }

        #endregion // Methods
    }
}