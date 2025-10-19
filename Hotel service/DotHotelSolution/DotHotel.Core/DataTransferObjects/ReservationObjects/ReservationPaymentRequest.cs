using DotHotel.Core.Enums;

namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public class ReservationPaymentRequest
    {
        public Guid ReservationId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
