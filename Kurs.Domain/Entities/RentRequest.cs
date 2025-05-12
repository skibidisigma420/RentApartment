using RentApartments.Domain.ValueObjects;
using RentApartments.Domain.Entities.Base;
using RentApartments.Domain.Entities;
using System;

namespace RentApartments.Domain.Entities
{
    public enum RentRequestStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class RentRequest : Entity<Guid>
    {
        #region Properties

        public Apartment Apartment { get; }
        public Tenant Tenant { get; }
        public Landlord Landlord { get; }
        public DateTime RequestDate { get; }
        public RentRequestStatus Status { get; private set; }
        public string? Message { get; }

        #endregion

        #region Constructor

        protected RentRequest() { }

        public RentRequest(Apartment apartment, Tenant tenant, Landlord landlord, string? message = null)
            : base(Guid.NewGuid())
        {
            Apartment = apartment ?? throw new ArgumentNullException(nameof(apartment));
            Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
            Landlord = landlord ?? throw new ArgumentNullException(nameof(landlord));
            RequestDate = DateTime.UtcNow;
            Status = RentRequestStatus.Pending;
            Message = message;
        }

        #endregion

        #region Methods

        public void Approve()
        {
            if (Status != RentRequestStatus.Pending)
                throw new InvalidOperationException("Request has already been processed.");

            Status = RentRequestStatus.Approved;
        }

        public void Reject()
        {
            if (Status != RentRequestStatus.Pending)
                throw new InvalidOperationException("Request has already been processed.");

            Status = RentRequestStatus.Rejected;
        }

        #endregion
    }
}
