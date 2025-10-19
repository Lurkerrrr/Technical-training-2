using DotHotel.Core.Enums;

namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public class ReservationSearchOptions
    {
        public ReservationFilter? Filter { get; set; }
        public ReservationSortOptions SortBy { get; set; } = ReservationSortOptions.CheckInDate;
        public SortOrderOptions SortOrder { get; set; } = SortOrderOptions.ASC;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
