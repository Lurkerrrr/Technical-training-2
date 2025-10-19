using System.Linq.Expressions;
using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Querying;

namespace DotHotel.Core.QueryBuilders.ApartmentFilterRules
{
    public class MinGuestsRule : IFilterRule<Apartment, ApartmentFilter>
    {
        public bool IsApplicable(ApartmentFilter filter) => filter.MinGuests.HasValue;

        public Expression<Func<Apartment, bool>> BuildPredicate(ApartmentFilter filter) => a => a.Capacity >= filter.MinGuests!.Value;
    }
}
