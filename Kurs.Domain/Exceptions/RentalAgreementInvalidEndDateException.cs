using RentApartments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApartments.Domain.Exceptions
{
    public class RentalAgreementInvalidEndDateException : InvalidOperationException
    {
        public RentalAgreement Agreement { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public RentalAgreementInvalidEndDateException(RentalAgreement agreement, DateTime startDate, DateTime endDate)
            : base($"Invalid end date '{endDate}'. It cannot be before the start date '{startDate}'.")
        {
            Agreement = agreement;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
