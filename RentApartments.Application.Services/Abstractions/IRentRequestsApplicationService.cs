using RentApartments.Application.Models.RentRequest;

namespace RentApartments.Application.Services.Abstractions
{
    public interface IRentRequestsApplicationService
    {
        Task<IEnumerable<RentRequestModel>> GetRequestsByApartmentIdAsync(Guid apartmentId, CancellationToken cancellationToken);

        Task<RentRequestModel?> CreateRentRequestAsync(CreateRentRequestModel requestInformation, CancellationToken cancellationToken);

        Task<bool> ApproveRequestAsync(Guid requestId, CancellationToken cancellationToken);

        Task<bool> RejectRequestAsync(Guid requestId, CancellationToken cancellationToken);
    }
}
