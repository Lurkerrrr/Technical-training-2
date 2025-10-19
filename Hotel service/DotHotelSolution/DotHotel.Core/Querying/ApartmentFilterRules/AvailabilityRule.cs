using System.Linq.Expressions;
using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Querying;

namespace DotHotel.Core.QueryBuilders.ApartmentFilterRules
{
    public class AvailabilityRule : IFilterRule<Apartment, ApartmentFilter>
    {
        public bool IsApplicable(ApartmentFilter filter) => filter.AvailableFrom.HasValue || filter.AvailableTo.HasValue;

        public Expression<Func<Apartment, bool>> BuildPredicate(ApartmentFilter filter)
        {
            var from = filter.AvailableFrom ?? DateTime.MinValue;
            var to = filter.AvailableTo ?? DateTime.MaxValue;

            return a => !a.Reservations.Any(r => r.CheckInDate < to && r.CheckOutDate > from);
        }
    }
}
