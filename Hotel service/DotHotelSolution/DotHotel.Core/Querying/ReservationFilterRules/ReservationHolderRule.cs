using System.Linq.Expressions;
using DotHotel.Core.DataTransferObjects.ReservationObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Querying;

namespace DotHotel.Core.QueryBuilders.ReservationFilterRules
{
    internal class ReservationHolderRule : IFilterRule<Reservation, ReservationFilter>
    {
        public bool IsApplicable(ReservationFilter filter) => filter.ReservationHolderId.HasValue;

        public Expression<Func<Reservation, bool>> BuildPredicate(ReservationFilter filter) => a => a.ReservationHolderId == filter.ReservationHolderId!.Value;
    }
}
