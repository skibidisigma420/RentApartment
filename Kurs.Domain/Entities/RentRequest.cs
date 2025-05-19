using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.Entities.Base;
using RentApartments.Domain.Enums;
using RentApartments.Domain.Exceptions;

namespace RentApartments.Domain.Entities
{
    /// <summary>
    /// Класс, представляющий запрос на аренду квартиры.
    /// Наследуется от Entity<Guid>.
    /// </summary>
    public class RentRequest : Entity<Guid>
    {
        #region Properties

        // Квартира, на которую подается запрос (только для чтения)
        public Apartment Apartment { get; }

        // Арендатор, подающий запрос (только для чтения)
        public Tenant Tenant { get; }

        // Арендодатель, который рассматривает запрос (только для чтения)
        public Landlord Landlord { get; }

        // Дата и время подачи запроса (только для чтения)
        public DateTime RequestDate { get; }

        // Текущий статус запроса 
        public RentRequestStatus Status { get; private set; }

        // Дополнительное сообщение к запросу (необязательное поле)
        public string? Message { get; }

        #endregion

        #region Constructors

        // Защищённый конструктор без параметров (для EF Core)
        protected RentRequest() { }

        /// <summary>
        /// Публичный конструктор — автоматически генерирует идентификатор.
        /// </summary>
        /// <param name="apartment">Квартира, на которую подается запрос.</param>
        /// <param name="tenant">Арендатор, подающий запрос.</param>
        /// <param name="landlord">Арендодатель, рассматривающий запрос.</param>
        /// <param name="message">Дополнительное сообщение.</param>
        public RentRequest(Apartment apartment, Tenant tenant, Landlord landlord, string? message = null)
            : this(Guid.NewGuid(), apartment, tenant, landlord, DateTime.UtcNow, message)
        {
        }

        /// <summary>
        /// Основной конструктор с возможностью задать ID и дату запроса.
        /// </summary>
        /// <param name="id">Идентификатор запроса.</param>
        /// <param name="apartment">Квартира, на которую подается запрос.</param>
        /// <param name="tenant">Арендатор, подающий запрос.</param>
        /// <param name="landlord">Арендодатель, рассматривающий запрос.</param>
        /// <param name="requestDate">Дата и время запроса.</param>
        /// <param name="message">Дополнительное сообщение.</param>
        protected RentRequest(Guid id, Apartment apartment, Tenant tenant, Landlord landlord, DateTime requestDate, string? message = null)
     : base(id)
        {
            Apartment = apartment ?? throw new ArgumentNullException(nameof(apartment));
            Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
            Landlord = landlord ?? throw new ArgumentNullException(nameof(landlord));

            // Проверка: квартира должна принадлежать арендодателю
            if (!landlord.ActiveApartments.Any(a => a.Id == apartment.Id))
                throw new LandlordDoesNotOwnApartmentException(landlord, apartment);

            // Проверка: квартира должна быть доступна
            if (apartment.Status != ApartmentStatus.Available)
                throw new ApartmentIsNotAvailableException(apartment);

            RequestDate = requestDate;
            Status = RentRequestStatus.Pending;
            Message = message;
        }


        #endregion

        #region Methods

        // Метод для одобрения запроса на аренду
        public void Approve()
        {
            if (Status != RentRequestStatus.Pending)
                throw new InvalidOperationException("Request has already been processed.");

            Status = RentRequestStatus.Approved;
        }

        // Метод для отклонения запроса на аренду
        public void Reject()
        {
            if (Status != RentRequestStatus.Pending)
                throw new InvalidOperationException("Request has already been processed.");

            Status = RentRequestStatus.Rejected;
        }

        #endregion
    }
}
