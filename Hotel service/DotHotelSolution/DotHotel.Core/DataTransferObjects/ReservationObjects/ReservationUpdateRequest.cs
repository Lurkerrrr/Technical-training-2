using DotHotel.Core.Domain.Models.HotelModels;

namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public class ReservationUpdateRequest : ReservationRequestBase
    {
        public Guid Id { get; set; }

        public override Reservation ToReservation(decimal totalPriceUsd)
        {
            return new Reservation
            {
                Id = Id,
                CheckInDate = CheckInDate!.Value,
                CheckOutDate = CheckOutDate!.Value,
                GuestCount = GuestCount,
                TotalPriceUsd = totalPriceUsd,
                ReservationHolderId = ReservationHolderId,
                ApartmentId = ApartmentId
            };
        }
    }

    public static class ReservationUpdateRequestExtensions
    {
        public static ReservationUpdateRequest ToReservationUpdateRequest(this ReservationResponse reservation)
        {
            return new ReservationUpdateRequest
            {
                Id = reservation.Id,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                GuestCount = reservation.GuestCount,
                ReservationHolderId = reservation.ReservationHolderId,
                ApartmentId = reservation.ApartmentId
            };
        }
    }
}
