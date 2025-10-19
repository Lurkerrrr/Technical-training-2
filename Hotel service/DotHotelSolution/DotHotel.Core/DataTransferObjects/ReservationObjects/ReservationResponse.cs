using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Domain.Models.IdentityModels;
using DotHotel.Core.Enums;

namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int GuestCount { get; set; }
        public decimal TotalPriceUsd { get; set; }

        public ReservationStatus ReservationStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public Guid ReservationHolderId { get; set; }
        public ApplicationUser ReservationHolder { get; set; } = null!;

        public Guid ApartmentId { get; set; }
        public ApartmentResponse Apartment { get; set; } = null!;
    }

    public static class ReservationResponseExtensions
    {
        public static ReservationResponse ToReservationResponse(this Reservation reservation)
        {
            return new ReservationResponse
            {
                Id = reservation.Id,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                GuestCount = reservation.GuestCount,
                TotalPriceUsd = reservation.TotalPriceUsd,
                ReservationStatus = reservation.ReservationStatus,
                PaymentMethod = reservation.PaymentMethod,
                ReservationHolderId = reservation.ReservationHolderId,
                ReservationHolder = reservation.ReservationHolder,
                ApartmentId = reservation.ApartmentId,
                Apartment = reservation.Apartment.ToApartmentResponse()
            };
        }
    }
}
