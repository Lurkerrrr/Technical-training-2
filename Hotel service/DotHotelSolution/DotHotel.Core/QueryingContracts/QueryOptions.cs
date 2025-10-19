using System.Linq.Expressions;

namespace DotHotel.Core.Querying
{
    public class QueryOptions<T>
    {
        public Expression<Func<T, bool>>? Filter { get; set; }
        public string? Sort { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
