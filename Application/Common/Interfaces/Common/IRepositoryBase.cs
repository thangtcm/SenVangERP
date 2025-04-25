using System.Linq.Expressions;
using Application.Bases;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Common.Interfaces.Common
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<TResult>> GetData<TResult>(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Expression<Func<T, TResult>>? selector = null,
            bool trackingChanges = false,
            bool ignoreQueryFilters = false,
            Action<IQueryable<T>>? configureIncludes = null);

        Task<IEnumerable<TDto>> GetProjectedData<TDto>(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool trackingChanges = false,
            bool ignoreQueryFilters = false,
            Action<IQueryable<T>>? configureIncludes = null)
            where TDto : class;

        Task<TDto?> GetSingleProjectedAsync<TDto>(
            Expression<Func<T, bool>> predicate,
            bool trackingChanges = false,
            bool ignoreQueryFilters = false,
            Action<IQueryable<T>>? configureIncludes = null)
            where TDto : class;

        Task<T?> GetSingleAsync(
            Expression<Func<T, bool>> predicate,
            bool trackingChanges = false,
            bool ignoreQueryFilters = false,
            Action<IQueryable<T>>? configureIncludes = null);
        ValueTask<T?> GetAsync(params object[] keyValues);
        T Insert(T entity);
        Task<T> InsertAsync(T entity);
        IEnumerable<T> InsertRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        Task<int> UpdateAsync(
                Expression<Func<T, bool>> predicate,
                Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setters,
                CancellationToken cancellationToken = default);
        IEnumerable<T> UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> filter);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetChildsAsync(Expression<Func<T, bool>> include, Expression<Func<T, bool>> filter);
        public Task<(List<TDTO> data, long totalCount)> FilterData<TDTO>(Func<IQueryable<T>, IQueryable<T>>? filterFunc, BaseFilteration parameters, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null) where TDTO : class;
        bool isExist(Expression<Func<T, bool>> filter);
        void Commit();
        void RollBack();
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        (IQueryable<T> data, long totalCount, bool HasNextPage) FilterData(Func<IQueryable<T>, IQueryable<T>> filterFunc, BaseFilteration parameters, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate);
        Task SaveChangesAsync();
        void SaveChanges();
        Task<int> DeleteAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<TResult>> GetGroupedData<TKey, TResult>(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, TKey>>? groupBy = null,
            Expression<Func<IGrouping<TKey, T>, TResult>>? selector = null,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>>? orderBy = null,
            int? take = null,
            bool trackingChanges = false,
            bool ignoreQueryFilters = false);
        Task<bool> AnyExistAsync(
            Expression<Func<T, bool>> predicate,
            bool ignoreQueryFilters = false);
    }
}
