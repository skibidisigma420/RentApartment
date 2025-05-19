using RentApartments.Domain.Exceptions;
using RentApartments.Domain.Entities.Base;
using RentApartments.Domain.ValueObjects;
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
        ///приватная коллекция для хранения всех квартир принадлежащих арендодателю
        ///ICollection<Apartment> изменяемая коллекция квартир
        ///инициализируется пустым списком
        /// </summary>
        private readonly ICollection<Apartment> _apartments = [];

        #endregion 

        #region Properties

        /// <summary> 
        ///имя пользователя арендодателя
        ///Username value object
     
        ///проверка при создании выбрасывает ArgumentNullValueException если username null
        /// </summary>
        public Username Username { get; private set; } = username ?? throw new ArgumentNullValueException(nameof(username));

        /// <summary>
        /// только для чтения коллекция активных (доступных) квартир арендодателя
        /// возвращает IReadOnlyCollection<Apartment> неизменяемую коллекцию
        /// фильтрация только квартиры со статусом ApartmentStatus.Available
        /// преобразование ToList().AsReadOnly() для безопасного возврата
        /// </summary>
        public IReadOnlyCollection<Apartment> ActiveApartments =>
            _apartments.Where(apartment => apartment.Status == ApartmentStatus.Available).ToList().AsReadOnly();

        #endregion 



        #region Methods

        /// <summary>
        /// добавляет квартиру в список арендодателя
        /// принимает apartment объект квартиры для добавления
        /// проверки:
        /// если квартира уже есть в списке, выбрасывает InvalidOperationException
        /// действие: добавляет квартиру в коллекцию _apartments
        /// </summary>
        public void AddApartment(Apartment apartment)
        {
            if (_apartments.Contains(apartment))
                throw new InvalidOperationException("This apartment is already listed.");

            _apartments.Add(apartment);
        }

        /// <summary>
        /// удаляет квартиру из списка арендодателя
        /// принимает apartment объект квартиры для удаления
        /// возвращает: 
        /// true если квартира найдена и удалена
        /// false если квартира не найдена
        /// логика:
        /// ищет квартиру по Id (не по ссылке)
        /// если найдена - удаляет и возвращает true
        /// если не найдена - возвращает false
        /// </summary>
        public bool RemoveApartment(Apartment apartment)
        {
            var apartmentToRemove = _apartments.FirstOrDefault(a => a.Id == apartment.Id);
            if (apartmentToRemove == null)
                return false;

            _apartments.Remove(apartmentToRemove);
            return true;
        }

        /// зачем это
        public void ChangeApartmentStatus(Apartment apartment, ApartmentStatus newStatus)
        {
            var apartmentToUpdate = _apartments.FirstOrDefault(a => a.Id == apartment.Id)
                ?? throw new ApartmentDoesNotExistException(apartment);

            apartmentToUpdate.ChangeStatus(newStatus);
        }

        /// <summary>
        /// обновляет описание квартиры арендодателя
        /// принимает:
        /// apartment квартира для обновления
        /// newDescription новое описание 
        /// если квартира не принадлежит арендодателю, выбрасывает ApartmentDoesNotExistException
        /// вызывает метод UpdateDescription у найденной квартиры
        /// внутри Apartment происходит проверка на null newDescription
        /// </summary>
        public void UpdateApartmentDetails(Apartment apartment, string newDescription)
        {
            var apartmentToUpdate = _apartments.FirstOrDefault(a => a.Id == apartment.Id)
                ?? throw new ApartmentDoesNotExistException(apartment);

            apartmentToUpdate.UpdateDescription(newDescription);
        }

        #endregion // Methods
    }
}