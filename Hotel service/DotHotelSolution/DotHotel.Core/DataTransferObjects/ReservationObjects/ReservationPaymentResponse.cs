using DotHotel.Core.Enums;

namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public class ReservationPaymentResponse
    {
        public Guid ReservationId { get; set; }
        public ReservationStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
