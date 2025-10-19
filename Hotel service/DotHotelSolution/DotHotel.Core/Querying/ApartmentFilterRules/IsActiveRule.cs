using System.Linq.Expressions;
using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Querying;

namespace DotHotel.Core.QueryBuilders.ApartmentFilterRules
{
    public class IsActiveRule : IFilterRule<Apartment, ApartmentFilter>
    {
        public bool IsApplicable(ApartmentFilter filter) => filter.MinRating.HasValue;

        public Expression<Func<Apartment, bool>> BuildPredicate(ApartmentFilter filter) => a => a.IsActive == filter.IsActive;
    }
}
