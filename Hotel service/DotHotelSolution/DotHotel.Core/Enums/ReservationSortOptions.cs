using System.ComponentModel.DataAnnotations;

namespace DotHotel.Core.Enums
{
    public enum ReservationSortOptions
    {
        [Display(Name = "Check In Date")]
        CheckInDate,
        Status,
        [Display(Name = "Total Price")]
        TotalPrice,
        [Display(Name = "Payment Method")]
        PaymentMethod
    }
}
