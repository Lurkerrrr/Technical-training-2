using System.Linq.Expressions;
using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Querying;

namespace DotHotel.Core.QueryBuilders.ApartmentFilterRules
{
    public class MaxPriceRule : IFilterRule<Apartment, ApartmentFilter>
    {
        public bool IsApplicable(ApartmentFilter filter) => filter.MaxPrice.HasValue;

        public Expression<Func<Apartment, bool>> BuildPredicate(ApartmentFilter filter) => a => a.PricePerNightPerPersonUsd <= filter.MaxPrice!.Value;
    }
}
