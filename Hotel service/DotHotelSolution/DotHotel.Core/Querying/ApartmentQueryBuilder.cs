using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Enums;
using DotHotel.Core.QueryBuilders.ApartmentFilterRules;
using DotHotel.Core.Querying;
using LinqKit;

namespace DotHotel.Core.QueryBuilders
{
    public class ApartmentQueryBuilder : IQueryBuilder<ApartmentSearchOptions, Apartment>
    {
        private static readonly Dictionary<ApartmentSortOptions, string> _sorts = new Dictionary<ApartmentSortOptions, string>
        {
            { ApartmentSortOptions.Price, nameof(Apartment.PricePerNightPerPersonUsd) },
            { ApartmentSortOptions.Rating, nameof(Apartment.Rating) }
        };

        private static readonly List<IFilterRule<Apartment, ApartmentFilter>> _filterRules = new List<IFilterRule<Apartment, ApartmentFilter>>
        {
            new MinPriceRule(),
            new MaxPriceRule(),
            new MinGuestsRule(),
            new MinRatingRule(),
            new AvailabilityRule(),
            new IsActiveRule()
        };

        public QueryOptions<Apartment>? BuildQuery(ApartmentSearchOptions? searchOptions)
        {
            if (searchOptions == null)
            {
                return null;
            }

            var filterPredicate = PredicateBuilder.New<Apartment>(true);

            if (searchOptions.Filter != null)
            {
                filterPredicate = _filterRules
                    .Where(rule => rule.IsApplicable(searchOptions.Filter))
                    .Aggregate(filterPredicate, (predicate, rule) => predicate.And(rule.BuildPredicate(searchOptions.Filter)));
            }

            var sortExpression = _sorts[searchOptions.SortBy];
            var sort = $"{_sorts[searchOptions.SortBy]} {searchOptions.SortOrder}";

            return new QueryOptions<Apartment>
            {
                Filter = filterPredicate,
                Sort = sort,
                PageNumber = searchOptions.PageNumber,
                PageSize = searchOptions.PageSize
            };
        }
    }
}
