namespace DotHotel.Core.Querying
{
    public interface IQueryBuilder<TSearch, TEntity>
    {
        QueryOptions<TEntity>? BuildQuery(TSearch? searchOptions);
    }
}
