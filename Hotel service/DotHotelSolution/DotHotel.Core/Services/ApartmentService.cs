using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Domain.RepositoryContracts;
using DotHotel.Core.Querying;
using DotHotel.Core.ServiceContracts;

namespace DotHotel.Core.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IQueryBuilder<ApartmentSearchOptions, Apartment> _queryBuilder;

        public ApartmentService(IApartmentRepository apartmentRepository, IQueryBuilder<ApartmentSearchOptions, Apartment> queryBuilder)
        {
            _apartmentRepository = apartmentRepository ?? throw new ArgumentNullException(nameof(apartmentRepository));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
        }

        public async Task<List<ApartmentResponse>> GetApartmentsAsync(ApartmentSearchOptions? searchOptions)
        {
            var queryOptions = _queryBuilder?.BuildQuery(searchOptions);

            var apartments = await _apartmentRepository.GetApartmentsAsync(queryOptions);
            return apartments.Select(a => a.ToApartmentResponse()).ToList();
        }

        public async Task<ApartmentResponse?> GetApartmentByIdAsync(Guid? apartmentId)
        {
            if (apartmentId == null || apartmentId == Guid.Empty)
            {
                throw new ArgumentException("Apartment ID cannot be empty.", nameof(apartmentId));
            }

            var apartment = await _apartmentRepository.GetApartmentByIdAsync(apartmentId.Value);
            return apartment?.ToApartmentResponse();
        }

        public async Task<ApartmentResponse> AddApartmentAsync(ApartmentAddRequest? apartmentAddRequest)
        {
            if (apartmentAddRequest == null)
            {
                throw new ArgumentNullException(nameof(apartmentAddRequest));
            }

            var addedApartment = await _apartmentRepository.AddApartmentAsync(apartmentAddRequest.ToApartment());
            return addedApartment.ToApartmentResponse();
        }

        public async Task<ApartmentResponse> UpdateApartmentAsync(ApartmentUpdateRequest? apartmentUpdateRequest)
        {
            if (apartmentUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(apartmentUpdateRequest));
            }

            var updatedApartment = await _apartmentRepository.UpdateApartmentAsync(apartmentUpdateRequest.ToApartment());
            return updatedApartment.ToApartmentResponse();
        }

        public async Task<bool> DeleteApartmentAsync(Guid? apartmentId)
        {
            if (apartmentId == null || apartmentId == Guid.Empty)
            {
                throw new ArgumentException("Apartment ID cannot be empty.", nameof(apartmentId));
            }

            var success = await _apartmentRepository.DeleteApartmentAsync(apartmentId.Value);
            return success;
        }

        public async Task<bool> ValidateGuestCountAsync(Guid apartmentId, int guestCount)
        {
            if (apartmentId == Guid.Empty)
            {
                return false;
            }

            if (guestCount <= 0)
            {
                return false;
            }

            var apartmentCapacity = await _apartmentRepository.GetApartmentCapacityAsync(apartmentId);
            if (apartmentCapacity == null)
            {
                return false;
            }

            return guestCount <= apartmentCapacity.Value;
        }
    }
}
