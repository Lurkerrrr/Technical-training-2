using System.Linq.Expressions;

namespace DotHotel.Core.Querying
{
    public interface IFilterRule<TEntity, TFilter>
    {
        bool IsApplicable(TFilter filter);
        Expression<Func<TEntity, bool>> BuildPredicate(TFilter filter);
    }
}
