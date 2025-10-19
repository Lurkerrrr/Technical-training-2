using DotHotel.Core.Domain.Models.IdentityModels;
using DotHotel.Core.Enums;

namespace DotHotel.Core.Domain.Models.HotelModels
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int GuestCount { get; set; }
        public decimal TotalPriceUsd { get; set; }

        public ReservationStatus ReservationStatus { get; set; } = ReservationStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.None;

        public Guid ReservationHolderId { get; set; }
        public ApplicationUser ReservationHolder { get; set; } = null!;

        public Guid ApartmentId { get; set; }
        public Apartment Apartment { get; set; } = null!;

        public bool CanCancel()
        {
            if (ReservationStatus != ReservationStatus.Pending)
            {
                return false;
            }

            return true;
        }

        public void Cancel()
        {
            ReservationStatus = ReservationStatus.Cancelled;
        }

        public bool CanProcessPayment()
        {
            if (ReservationStatus != ReservationStatus.Pending)
            {
                return false;
            }

            return true;
        }

        public void ProcessPayment(PaymentMethod paymentMethod)
        {
            ReservationStatus = ReservationStatus.Confirmed;
            PaymentMethod = paymentMethod;
        }

        public bool CanCheckIn()
        {
            if (ReservationStatus != ReservationStatus.Confirmed && CheckInDate.Date != DateTime.UtcNow.Date)
            {
                return false;
            }

            return true;
        }

        public void CheckIn()
        {
            ReservationStatus = ReservationStatus.CheckedIn;
        }

        public bool CanCheckOut()
        {
            if (ReservationStatus != ReservationStatus.CheckedIn && CheckOutDate.Date != DateTime.UtcNow.Date)
            {
                return false;
            }

            return true;
        }

        public void CheckOut()
        {
            ReservationStatus = ReservationStatus.CheckedOut;
        }
    }
}
