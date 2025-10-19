using DotHotel.Core.DataTransferObjects.ReservationObjects;

namespace DotHotel.Core.ServiceContracts
{
    public interface IReservationService
    {
        Task<List<ReservationResponse>> GetReservationsAsync(ReservationSearchOptions? searchOptions);

        Task<ReservationResponse?> GetReservationByIdAsync(Guid? reservationId);

        Task<ReservationResponse> CreateReservationAsync(ReservationAddRequest? reservationAddRequest);

        Task<ReservationResponse> UpdateReservationAsync(ReservationUpdateRequest? reservationUpdateRequest);

        Task<bool> CancelReservationAsync(Guid? reservationId);

        Task<bool> ProcessReservationPaymentAsync(ReservationPaymentRequest? reservationId);

        Task<bool> ConfirmReservationCheckInAsync(Guid? reservationId);

        Task<bool> ConfirmReservationCheckOutAsync(Guid? reservationId);

        Task<bool> ValidateReservationAddRequestAsync(ReservationAddRequest? reservationAddRequest);

        Task<bool> ValidateReservationDatesAsync(Guid apartmentId, DateTime checkInDate, DateTime checkOutDate);
    }
}
