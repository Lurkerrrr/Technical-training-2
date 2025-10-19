using System.ComponentModel.DataAnnotations;
using DotHotel.Core.ValidationAttributes;

namespace DotHotel.Core.DataTransferObjects.ApartmentObjects
{
    public class ApartmentFilter
    {
        [Display(Name = "Min Price")]
        public decimal? MinPrice { get; set; }
        [Display(Name = "Max Price")]
        public decimal? MaxPrice { get; set; }

        [Display(Name = "Min Guests")]
        public int? MinGuests { get; set; }

        [Display(Name = "Min Rating")]
        public double? MinRating { get; set; }

        [Display(Name = "Available From")]
        [DateNotInPast]
        [RequiredWith(nameof(AvailableTo))]
        public DateTime? AvailableFrom { get; set; }

        [Display(Name = "Available To")]
        [DateNotInPast]
        [MinimumDaysAfter(nameof(AvailableFrom), 1)]
        [RequiredWith(nameof(AvailableFrom))]
        public DateTime? AvailableTo { get; set; }

        public bool? IsActive { get; set; }
    }
}
