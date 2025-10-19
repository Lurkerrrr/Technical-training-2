using DotHotel.Core.DataTransferObjects.ApartmentObjects;

namespace DotHotel.Core.ServiceContracts
{
    public interface IApartmentService
    {
        Task<List<ApartmentResponse>> GetApartmentsAsync(ApartmentSearchOptions? searchOptions);

        Task<ApartmentResponse?> GetApartmentByIdAsync(Guid? apartmentId);

        Task<ApartmentResponse> AddApartmentAsync(ApartmentAddRequest? apartmentAddRequest);

        Task<ApartmentResponse> UpdateApartmentAsync(ApartmentUpdateRequest? apartmentUpdateRequest);

        Task<bool> DeleteApartmentAsync(Guid? apartmentId);

        Task<bool> ValidateGuestCountAsync(Guid apartmentId, int guestCount);
    }
}
