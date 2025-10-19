using System.ComponentModel.DataAnnotations;

namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public class ReservationPriceCalculationRequest
    {
        [Required]
        public Guid ApartmentId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int GuestCount { get; set; }
    }

    public static class ReservationPriceCalculationRequestExtensions
    {
        public static ReservationPriceCalculationRequest ToReservationPriceCalculationRequest(this ReservationRequestBase reservation)
        {
            return new ReservationPriceCalculationRequest
            {
                ApartmentId = reservation.ApartmentId,
                CheckInDate = reservation.CheckInDate!.Value,
                CheckOutDate = reservation.CheckOutDate!.Value,
                GuestCount = reservation.GuestCount
            };
        }
    }
}
