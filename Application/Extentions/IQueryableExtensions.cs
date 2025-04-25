using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Application.Extentions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string propertyName, bool ascending)
        {
            return ApplyOrdering(query, propertyName, ascending, isThenBy: false);
        }

        public static IQueryable<T> ThenByDynamic<T>(this IQueryable<T> query, string propertyName, bool ascending)
        {
            return ApplyOrdering(query, propertyName, ascending, isThenBy: true);
        }

        private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string propertyName, bool ascending, bool isThenBy)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = isThenBy
                ? (ascending ? "ThenBy" : "ThenByDescending")
                : (ascending ? "OrderBy" : "OrderByDescending");

            var result = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                query.Expression,
                lambda);

            return query.Provider.CreateQuery<T>(result);
        }
    }
}
