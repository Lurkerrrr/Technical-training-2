using DotHotel.Core.Domain.Models.HotelModels;

namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public class ReservationAddRequest : ReservationRequestBase
    {
        public override Reservation ToReservation(decimal totalPriceUsd)
        {
            return new Reservation
            {
                CheckInDate = CheckInDate!.Value,
                CheckOutDate = CheckOutDate!.Value,
                GuestCount = GuestCount,
                TotalPriceUsd = totalPriceUsd,
                ReservationHolderId = ReservationHolderId,
                ApartmentId = ApartmentId
            };
        }
    }
}
