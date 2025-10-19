using DotHotel.Core.Enums;

namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public class ReservationFilter
    {
        public ReservationStatus? ReservationStatus { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public Guid? ReservationHolderId { get; set; }
    }
}
