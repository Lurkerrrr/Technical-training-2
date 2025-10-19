using DotHotel.Core.DataTransferObjects.ReservationObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Domain.RepositoryContracts;
using DotHotel.Core.Querying;
using DotHotel.Core.ServiceContracts;

namespace DotHotel.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IQueryBuilder<ReservationSearchOptions, Reservation> _queryBuilder;

        private readonly IPricingService _pricingService;
        private readonly IApartmentService _apartmentService;

        public ReservationService(IReservationRepository reservationRepository, IQueryBuilder<ReservationSearchOptions, Reservation> queryBuilder, IPricingService pricingService, IApartmentService apartmentService)
        {
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
            _apartmentService = apartmentService ?? throw new ArgumentNullException(nameof(apartmentService));
        }

        public async Task<List<ReservationResponse>> GetReservationsAsync(ReservationSearchOptions? searchOptions)
        {
            var queryOptions = _queryBuilder?.BuildQuery(searchOptions);

            var reservations = await _reservationRepository.GetReservationsAsync(queryOptions);
            return reservations.Select(a => a.ToReservationResponse()).ToList();
        }

        public async Task<ReservationResponse?> GetReservationByIdAsync(Guid? reservationId)
        {
            if (reservationId == null || reservationId == Guid.Empty)
            {
                throw new ArgumentException("Reservation ID cannot be empty.", nameof(reservationId));
            }

            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId.Value);
            return reservation?.ToReservationResponse();
        }

        public async Task<ReservationResponse> CreateReservationAsync(ReservationAddRequest? reservationAddRequest)
        {
            if (reservationAddRequest == null)
            {
                throw new ArgumentNullException(nameof(reservationAddRequest));
            }

            var pricingResult = await _pricingService.CalculateTotalPriceAsync(reservationAddRequest.ToReservationPriceCalculationRequest());
            var addedReservation = await _reservationRepository.CreateReservationAsync(reservationAddRequest.ToReservation(pricingResult.TotalPriceUsd));
            return addedReservation.ToReservationResponse();
        }

        public async Task<ReservationResponse> UpdateReservationAsync(ReservationUpdateRequest? reservationUpdateRequest)
        {
            if (reservationUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(reservationUpdateRequest));
            }

            var pricingResult = await _pricingService.CalculateTotalPriceAsync(reservationUpdateRequest.ToReservationPriceCalculationRequest());
            var updatedReservation = await _reservationRepository.UpdateReservationAsync(reservationUpdateRequest.ToReservation(pricingResult.TotalPriceUsd));
            return updatedReservation.ToReservationResponse();
        }

        public async Task<bool> CancelReservationAsync(Guid? reservationId)
        {
            if (reservationId == null || reservationId == Guid.Empty)
            {
                throw new ArgumentException("Reservation ID cannot be empty.", nameof(reservationId));
            }

            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId.Value);

            if (reservation == null)
            {
                throw new ArgumentException($"Reservation with ID {reservationId.Value} not found.", nameof(reservationId));
            }

            var canCancel = reservation.CanCancel();

            if (!canCancel)
            {
                throw new InvalidOperationException($"Reservation with ID {reservationId} cannot be cancelled. Ensure that the reservation is in a valid state for cancelling.");
            }

            reservation.Cancel();

            await _reservationRepository.UpdateReservationAsync(reservation);

            return true;
        }

        public async Task<bool> ProcessReservationPaymentAsync(ReservationPaymentRequest? reservationPaymentRequest)
        {
            if (reservationPaymentRequest == null || reservationPaymentRequest?.ReservationId == null || reservationPaymentRequest.ReservationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(reservationPaymentRequest), "Reservation payment request cannot be null and must contain a valid reservation ID.");
            }

            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationPaymentRequest.ReservationId);

            if (reservation == null)
            {
                throw new ArgumentException($"Reservation with ID {reservationPaymentRequest.ReservationId} not found.", nameof(reservationPaymentRequest.ReservationId));
            }

            var canProcessPayment = reservation.CanProcessPayment();

            if (!canProcessPayment)
            {
                throw new InvalidOperationException($"Payment for reservation with ID {reservationPaymentRequest.ReservationId} cannot be completed. Ensure that the reservation is in a valid state for processing payment.");
            }

            reservation.ProcessPayment(reservationPaymentRequest.PaymentMethod);

            await _reservationRepository.UpdateReservationAsync(reservation);

            return true;
        }

        public async Task<bool> ConfirmReservationCheckInAsync(Guid? reservationId)
        {
            if (reservationId == null || reservationId == Guid.Empty)
            {
                throw new ArgumentException("Reservation ID cannot be empty.", nameof(reservationId));
            }

            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId.Value);

            if (reservation == null)
            {
                throw new ArgumentException($"Reservation with ID {reservationId.Value} not found.", nameof(reservationId));
            }

            var canCheckIn = reservation.CanCheckIn();

            if (canCheckIn)
            {
                throw new InvalidOperationException($"Reservation with ID {reservationId.Value} cannot be checked in. Ensure that the reservation is in a valid state for check-in.");
            }

            reservation.CheckIn();

            await _reservationRepository.UpdateReservationAsync(reservation);

            return true;
        }

        public async Task<bool> ConfirmReservationCheckOutAsync(Guid? reservationId)
        {
            if (reservationId == null || reservationId == Guid.Empty)
            {
                throw new ArgumentException("Reservation ID cannot be empty.", nameof(reservationId));
            }

            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId.Value);

            if (reservation == null)
            {
                throw new ArgumentException($"Reservation with ID {reservationId.Value} not found.", nameof(reservationId));
            }

            var canCheckOut = reservation.CanCheckOut();

            if (!canCheckOut)
            {
                throw new InvalidOperationException($"Reservation with ID {reservationId.Value} cannot be checked out. Ensure that the reservation is in a valid state for check-out.");
            }

            reservation.CheckOut();

            await _reservationRepository.UpdateReservationAsync(reservation);

            return true;
        }

        public async Task<bool> ValidateReservationAddRequestAsync(ReservationAddRequest? reservationAddRequest)
        {
            if (reservationAddRequest == null)
            {
                return false;
            }

            var isDatesValid = await ValidateReservationDatesAsync(reservationAddRequest.ApartmentId, reservationAddRequest.CheckInDate!.Value, reservationAddRequest.CheckOutDate!.Value);
            var isGuestCountValid = await _apartmentService.ValidateGuestCountAsync(reservationAddRequest.ApartmentId, reservationAddRequest.GuestCount);

            return isDatesValid && isGuestCountValid;
        }

        public async Task<bool> ValidateReservationDatesAsync(Guid apartmentId, DateTime checkInDate, DateTime checkOutDate)
        {
            if (apartmentId == Guid.Empty)
            {
                return false;
            }

            if (checkInDate == default || checkOutDate == default)
            {
                return false;
            }

            if (checkInDate >= checkOutDate)
            {
                return false;
            }

            if (checkInDate < DateTime.UtcNow.Date || checkOutDate < DateTime.UtcNow.Date)
            {
                return false;
            }

            var hasOverlappingReservations = await _reservationRepository.HasOverlappingReservationsAsync(
                apartmentId, checkInDate, checkOutDate);

            return !hasOverlappingReservations;
        }
    }
}
