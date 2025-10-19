using System.ComponentModel.DataAnnotations;

namespace DotHotel.Core.Enums
{
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        [Display(Name = "Checked In")]
        CheckedIn,
        [Display(Name = "Checked Out")]
        CheckedOut,
        Cancelled
    }
}
