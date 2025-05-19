using AutoMapper;
using RentApartments.Application.Models.RentRequest;
using RentApartments.Application.Services.Abstractions;
using RentApartments.Domain.Entities;
using RentApartments.Domain.Repositories.Abstractions;
using RentApartments.Domain.Enums;

namespace RentApartments.Application.Services
{
    public class RentRequestsApplicationService(
        IRentRequestRepository rentRequestRepository,
        IApartmentRepository apartmentRepository,
        ITenantRepository tenantRepository,
        ILandlordRepository landlordRepository,
        IMapper mapper) : IRentRequestsApplicationService
    {
        public async Task<IEnumerable<RentRequestModel>> GetRequestsByApartmentIdAsync(Guid apartmentId, CancellationToken cancellationToken = default)
        {
            var rentRequests = await rentRequestRepository.GetByApartmentIdAsync(apartmentId, cancellationToken);
            return rentRequests.Select(mapper.Map<RentRequestModel>);
        }

        public async Task<RentRequestModel?> CreateRentRequestAsync(CreateRentRequestModel requestInformation, CancellationToken cancellationToken = default)
        {
            var apartment = await apartmentRepository.GetByIdAsync(requestInformation.ApartmentId, cancellationToken);
            var tenant = await tenantRepository.GetByIdAsync(requestInformation.TenantId, cancellationToken);
            var landlord = await landlordRepository.GetByIdAsync(requestInformation.LandlordId, cancellationToken);

            if (apartment is null || tenant is null || landlord is null)
                return null;

            var rentRequest = new RentRequest(apartment, tenant, landlord, requestInformation.Message);

            var addedRequest = await rentRequestRepository.AddAsync(rentRequest, cancellationToken);
            await rentRequestRepository.SaveChangesAsync(cancellationToken);

            return addedRequest is null ? null : mapper.Map<RentRequestModel>(addedRequest);
        }

        public async Task<bool> ApproveRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var rentRequest = await rentRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (rentRequest is null || rentRequest.Status != RentRequestStatus.Pending)
                return false;

            rentRequest.Approve();
            await rentRequestRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> RejectRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var rentRequest = await rentRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (rentRequest is null || rentRequest.Status != RentRequestStatus.Pending)
                return false;

            rentRequest.Reject();
            await rentRequestRepository.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
