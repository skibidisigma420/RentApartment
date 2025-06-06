﻿using RentApartments.Domain.Exceptions;
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
        private readonly ICollection<RentRequest> _rentRequests = new List<RentRequest>();

        public Title Title { get; }
        public Description Description { get; private set; }
        public Address Address { get; }
        public Money MonthlyRent { get; }
        public Landlord Landlord { get; }
        public ApartmentStatus Status { get; private set; }

        public RentRequest? LastRequest => _rentRequests.OrderByDescending(r => r.RequestDate).FirstOrDefault();
        public bool IsAvailable => Status == ApartmentStatus.Available;

        #region Constructors

        // Для ORM / сериализации
        protected Apartment() { }

        /// <summary>
        /// Основной защищённый конструктор с полной инициализацией.
        /// Используется как точка централизованной валидации и создания.
        /// </summary>
        protected Apartment(
        Guid id,
        Title title,
        Description description,
        Address address,
        Money monthlyRent,
        Landlord landlord,
        ApartmentStatus status) : base(id)
        {
            if (monthlyRent.Value <= 0)
                throw new InvalidApartmentMonthlyRentException(this, monthlyRent);

            if (landlord?.ActiveApartments.Any(a => a.Address == address) == true && status == ApartmentStatus.Available)
                throw new ApartmentAlreadyRentedException(this, address);


            if (landlord == null)
                throw new ArgumentNullValueException(nameof(landlord));

            Title = title ?? throw new ArgumentNullValueException(nameof(title));
            Description = description ?? throw new ArgumentNullValueException(nameof(description));
            Address = address ?? throw new ArgumentNullValueException(nameof(address));
            MonthlyRent = monthlyRent ?? throw new ArgumentNullValueException(nameof(monthlyRent));
            Landlord = landlord;
            Status = status;
        }


        /// <summary>
        /// Публичный конструктор для создания новой квартиры.
        /// Устанавливает статус по умолчанию — Available.
        /// </summary>
        public Apartment(
            Guid id,
            Title title,
            Description description,
            Address address,
            Money monthlyRent,
            Landlord landlord
        ) : this(Guid.NewGuid(), title, description, address, monthlyRent, landlord, ApartmentStatus.Available)
        {
        }

        #endregion // Constructors


        public bool SetUnavailable()
        {
            if (!IsAvailable)
                throw new ApartmentIsNotAvailableException(this);

            Status = ApartmentStatus.Unavailable;
            return true;
        }

        public bool SetRented(Tenant tenant)
        {
            if (!IsAvailable)
                throw new ApartmentIsNotAvailableException(this);

            if (!_rentRequests.Any(r =>r.Status == RentRequestStatus.Approved && r.Tenant.Id == tenant.Id))
                throw new ApartmentRentRequestMissingException(this);

            Status = ApartmentStatus.Rented;
            return true;
        }

        public bool AddRentRequest(RentRequest request)
        {
            if (!ReferenceEquals(request.Apartment, this))
                throw new InvalidApartmentReferenceException(this, request);

            if (_rentRequests.Contains(request))
                throw new DuplicateRentRequestException(this, request);

            if (!IsAvailable)
                throw new ApartmentIsNotAvailableException(this);

            _rentRequests.Add(request);
            return true;
        }
        public void ChangeStatus(ApartmentStatus newStatus)
        {
            if (Status == newStatus)
                return;

            if (!IsStatusTransitionValid(newStatus))
                throw new InvalidApartmentStatusTransitionException(this, Status, newStatus);

            Status = newStatus;
        }

        private bool IsStatusTransitionValid(ApartmentStatus newStatus)
        {
            return (Status, newStatus) switch
            {
                // доступна → Арендована: требуется одобренный запрос
                (ApartmentStatus.Available, ApartmentStatus.Rented) =>
                    _rentRequests.Any(r => r.Status == RentRequestStatus.Approved),

                // Доступна → Недоступна: всегда разрешено
                (ApartmentStatus.Available, ApartmentStatus.Unavailable) => true,

                // Арендована → Доступна: разрешено (освобождение квартиры)
                (ApartmentStatus.Rented, ApartmentStatus.Available) => true,

                // Недоступна → Доступна: всегда разрешено
                (ApartmentStatus.Unavailable, ApartmentStatus.Available) => true,

                // Запрещённые переходы:
                (ApartmentStatus.Rented, ApartmentStatus.Unavailable) => false,    // Сначала завершите аренду
                (ApartmentStatus.Unavailable, ApartmentStatus.Rented) => false,    // Сначала сделайте доступной

                // Все остальные случаи (включая повтор статуса) запрещены
                _ => false
            };
        }

        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("Description cannot be empty or whitespace.", nameof(newDescription));

            Description = new Description(newDescription); 
        }


    }

}