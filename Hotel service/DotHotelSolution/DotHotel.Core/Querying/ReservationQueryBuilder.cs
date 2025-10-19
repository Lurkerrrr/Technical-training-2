using DotHotel.Core.DataTransferObjects.ReservationObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Enums;
using DotHotel.Core.QueryBuilders.ReservationFilterRules;
using DotHotel.Core.Querying;
using LinqKit;

namespace DotHotel.Core.QueryBuilders
{
    public class ReservationQueryBuilder : IQueryBuilder<ReservationSearchOptions, Reservation>
    {
        private static readonly Dictionary<ReservationSortOptions, string> _sorts = new Dictionary<ReservationSortOptions, string>
        {
            { ReservationSortOptions.CheckInDate, nameof(Reservation.CheckInDate) },
            { ReservationSortOptions.Status, nameof(Reservation.ReservationStatus) },
            { ReservationSortOptions.PaymentMethod, nameof(Reservation.PaymentMethod) },
            { ReservationSortOptions.TotalPrice, nameof(Reservation.TotalPriceUsd) }
        };

        private static readonly List<IFilterRule<Reservation, ReservationFilter>> _filterRules = new List<IFilterRule<Reservation, ReservationFilter>>
        {
            new StatusRule(),
            new PaymentMethodRule(),
            new ReservationHolderRule()
        };

        public QueryOptions<Reservation>? BuildQuery(ReservationSearchOptions? searchOptions)
        {
            if (searchOptions == null)
            {
                return null;
            }

            var filterPredicate = PredicateBuilder.New<Reservation>(true);

            if (searchOptions.Filter != null)
            {
                filterPredicate = _filterRules
                    .Where(rule => rule.IsApplicable(searchOptions.Filter))
                    .Aggregate(filterPredicate, (predicate, rule) => predicate.And(rule.BuildPredicate(searchOptions.Filter)));
            }

            var sortExpression = _sorts[searchOptions.SortBy];
            var sort = $"{_sorts[searchOptions.SortBy]} {searchOptions.SortOrder}";

            return new QueryOptions<Reservation>
            {
                Filter = filterPredicate,
                Sort = sort,
                PageNumber = searchOptions.PageNumber,
                PageSize = searchOptions.PageSize
            };
        }
    }
}
