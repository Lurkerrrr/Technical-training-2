using System.ComponentModel.DataAnnotations;
using DotHotel.Core.Enums;

namespace DotHotel.Core.DataTransferObjects.ApartmentObjects
{
    public class ApartmentSearchOptions
    {
        public ApartmentFilter? Filter { get; set; }
        [Display(Name = "Sort By")]
        public ApartmentSortOptions SortBy { get; set; } = ApartmentSortOptions.Rating;
        [Display(Name = "Sort Order")]
        public SortOrderOptions SortOrder { get; set; } = SortOrderOptions.DESC;
        [Display(Name = "Page")]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
