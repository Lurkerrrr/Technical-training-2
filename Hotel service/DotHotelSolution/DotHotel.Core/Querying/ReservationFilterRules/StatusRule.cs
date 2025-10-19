using System.Linq.Expressions;
using DotHotel.Core.DataTransferObjects.ReservationObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Querying;

namespace DotHotel.Core.QueryBuilders.ReservationFilterRules
{
    public class StatusRule : IFilterRule<Reservation, ReservationFilter>
    {
        public bool IsApplicable(ReservationFilter filter) => filter.ReservationStatus.HasValue;

        public Expression<Func<Reservation, bool>> BuildPredicate(ReservationFilter filter) => a => a.ReservationStatus == filter.ReservationStatus!.Value;
    }
}
