using System.Linq.Expressions;
using System.Reflection;
using Application.Bases;
using Application.Common.Interfaces.Common;
using Application.Extentions;
using AutoMapper;
using Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DataAccess.Repositories.Base
{
    public class RepositoryBase<TContext, T> : IRepositoryBase<T>
        where T : class
        where TContext : DbContext
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected virtual IQueryable<T> Table => _dbSet;
        protected virtual IQueryable<T> TableNoTracking => _dbSet.AsNoTrackingWithIdentityResolution();
        private readonly IMapper _mapper;
        public RepositoryBase(DbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentException(null, nameof(context));
            _dbSet = _context.Set<T>();
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> GetProjectedData<TDto>(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool trackingChanges = false,
            bool ignoreQueryFilters = false,
            Action<IQueryable<T>>? configureIncludes = null)
            where TDto : class
        {
            var set = _dbSet.AsQueryable();

            if (ignoreQueryFilters)
            {
                set = set.IgnoreQueryFilters();
            }

            configureIncludes?.Invoke(set);

            if (predicate != null)
            {
                set = set.Where(predicate);
            }

            if (orderBy != null)
            {
                set = orderBy(set);
            }

            set = trackingChanges ? set.AsTracking() : set.AsNoTrackingWithIdentityResolution();

            return await _mapper.ProjectTo<TDto>(set).ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetData<TResult>(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Expression<Func<T, TResult>>? selector = null,
            bool trackingChanges = false,
            bool ignoreQueryFilters = false,
            Action<IQueryable<T>>? configureIncludes = null)
        {
            var set = _dbSet.AsQueryable();

            if (ignoreQueryFilters)
            {
                set = set.IgnoreQueryFilters();
            }

            configureIncludes?.Invoke(set);

            if (predicate != null)
            {
                set = set.Where(predicate);
            }

            if (orderBy != null)
            {
                set = orderBy(set);
            }

            set = trackingChanges ? set.AsTracking() : set.AsNoTrackingWithIdentityResolution();

            IQueryable<TResult> result = selector != null
                ? set.Select(selector)
                : set.Select(x => (TResult)(object)x);

            return await result.ToListAsync();
        }

        public async Task<T?> GetSingleAsync(
            Expression<Func<T, bool>> predicate,
            bool trackingChanges = false,
            bool ignoreQueryFilters = false,
            Action<IQueryable<T>>? configureIncludes = null)
        {
            var set = _dbSet.AsQueryable();

            if (ignoreQueryFilters)
            {
                set = set.IgnoreQueryFilters();
            }

            if (predicate != null)
            {
                set = set.Where(predicate);
            }

            configureIncludes?.Invoke(set);

            set = trackingChanges ? set.AsTracking() : set.AsNoTrackingWithIdentityResolution();

            return await set.FirstOrDefaultAsync();
        }

        public async Task<TDto?> GetSingleProjectedAsync<TDto>(
        Expression<Func<T, bool>> predicate,
        bool trackingChanges = false,
        bool ignoreQueryFilters = false,
        Action<IQueryable<T>>? configureIncludes = null)
        where TDto : class
        {
            var set = _dbSet.AsQueryable();

            if (ignoreQueryFilters)
            {
                set = set.IgnoreQueryFilters();
            }

            configureIncludes?.Invoke(set);

            if (predicate != null)
            {
                set = set.Where(predicate);
            }

            set = trackingChanges ? set.AsTracking() : set.AsNoTrackingWithIdentityResolution();

            return await _mapper.ProjectTo<TDto>(set)
                .FirstOrDefaultAsync();
        }

        public ValueTask<T?> GetAsync(params object[] keyValues)
            => _dbSet.FindAsync(keyValues);
        public T Insert(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public async Task<T> InsertAsync(T entity)
        {

            await _dbSet.AddAsync(entity);
            //entity.Property(propertyName).CurrentValue = someValue;
            return entity;
        }

        public IEnumerable<T> InsertRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public async Task<int> UpdateAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setters,
        CancellationToken cancellationToken = default)
        {
            var set = _dbSet.AsQueryable();
            return await set.Where(predicate).ExecuteUpdateAsync(setters, cancellationToken);
        }

        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }
        public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            return entities;
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Attach(T entity)
        {
            _dbSet.Attach(entity);
        }

        public void AttachRange(IEnumerable<T> entities)
        {
            _dbSet.AttachRange(entities);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Count(filter);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.CountAsync(filter);
        }

        public async Task<IEnumerable<T>> GetChildsAsync(Expression<Func<T, bool>> include, Expression<Func<T, bool>> filter)
        {
            return await _dbSet
                      .Include(include)
                      .Where(filter).ToListAsync();
        }
        public bool isExist(Expression<Func<T, bool>> filter)
        {
            return _dbSet.AsNoTracking().Any(filter);
        }
        public void Commit()
        {
            _context.Database.CommitTransaction();

        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();

        }

        public IQueryable<T> GetTableAsTracking()
        {
            return _dbSet.AsQueryable();

        }
        public IQueryable<T> GetTableNoTracking()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public (IQueryable<T> data, long totalCount, bool HasNextPage) FilterData(Func<IQueryable<T>, IQueryable<T>> filterFunc, BaseFilteration parameters, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null)
        {
            if (parameters.Length > 50) parameters.Length = 50;
            if (parameters.Length < 1) parameters.Length = 1;
            if (parameters.Page < 0) parameters.Page = 0;
            var query = _dbSet.AsQueryable();

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            query = filterFunc(query);

            if (typeof(IModifiable).IsAssignableFrom(typeof(T)))
            {
                if (parameters.FromDate != 0)
                {
                    query = query.Where(item =>
                        ((IModifiable)item).ModifiedOn >= parameters.FromDate);
                }

                if (parameters.ToDate > 0)
                {
                    query = query.Where(item =>
                        ((IModifiable)item).ModifiedOn <= parameters.ToDate);
                }
            }

            if (parameters.OrderBy != null && parameters.OrderBy.Length > 0)
            {
                bool isFirstOrder = true;

                foreach (var orderBy in parameters.OrderBy)
                {
                    var propertyName = orderBy.Split(' ')[0];
                    propertyName = char.ToUpper(propertyName[0]) + propertyName[1..];
                    var property = typeof(T).GetProperty(propertyName);

                    if (property != null)
                    {
                        var ascending = !orderBy.EndsWith("desc", StringComparison.OrdinalIgnoreCase);

                        if (isFirstOrder)
                        {
                            query = query.OrderByDynamic(propertyName, ascending);
                            isFirstOrder = false;
                        }
                        else
                        {
                            query = query.ThenByDynamic(propertyName, ascending);
                        }
                    }
                }
            }
            else
            {
                query = query.OrderByDescending(q => ((IModifiable)q).ModifiedOn);
            }
            var pagedData = query
                .Skip((parameters.Page - 1) * parameters.Length)
                .Take(parameters.Length);
            var hasNextPage = _dbSet.Skip((parameters.Page + 1) * parameters.Length).Any();
            if (parameters.Select != null && parameters.Select.Length > 0)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var validProperties = typeof(T).GetProperties()
                    .Select(p => p.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
                var bindings = parameters.Select
                    .Where(name => validProperties.Contains(name))
                    .Select(name => Expression.PropertyOrField(parameter, name))
                    .Select(member => Expression.Bind(member.Member, member))
                    .Where(binding => binding != null);
                if (bindings.Any())
                {
                    var selector = Expression.Lambda<Func<T, T>>(
                        Expression.MemberInit(Expression.New(typeof(T)), bindings),
                        parameter
                    );
                    pagedData = pagedData.Select(selector);
                }
            }
            return (pagedData, pagedData.Count(), hasNextPage);
        }

        public async Task<(List<TDTO> data, long totalCount)> FilterData<TDTO>(Func<IQueryable<T>, IQueryable<T>>? filterFunc, BaseFilteration parameters, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null) where TDTO : class
        {
            if (parameters.Length > 50) parameters.Length = 50;
            if (parameters.Length < 1) parameters.Length = 1;
            if (parameters.Page < 0) parameters.Page = 0;
            var query = _dbSet.AsQueryable();

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }
            if (filterFunc != null)
            {
                query = filterFunc(query);
            }

            if (typeof(IModifiable).IsAssignableFrom(typeof(T)))
            {
                if (parameters.FromDate != 0)
                {
                    query = query.Where(item =>
                        ((IModifiable)item).ModifiedOn >= parameters.FromDate);
                }

                if (parameters.ToDate > 0)
                {
                    query = query.Where(item =>
                        ((IModifiable)item).ModifiedOn <= parameters.ToDate);
                }
            }

            if (parameters.OrderBy != null && parameters.OrderBy.Length > 0)
            {
                bool isFirstOrder = true;

                foreach (var orderBy in parameters.OrderBy)
                {
                    var propertyName = orderBy.Split(' ')[0];
                    propertyName = char.ToUpper(propertyName[0]) + propertyName.Substring(1);
                    var property = typeof(T).GetProperty(propertyName);

                    if (property != null)
                    {
                        var ascending = !orderBy.EndsWith("desc", StringComparison.OrdinalIgnoreCase);

                        if (isFirstOrder)
                        {
                            query = query.OrderByDynamic(propertyName, ascending).Distinct();
                            isFirstOrder = false;
                        }
                        else
                        {
                            query = query.ThenByDynamic(propertyName, ascending).Distinct();
                        }
                    }
                }
            }
            else
            {
                query = query.OrderByDescending(q => ((IModifiable)q).ModifiedOn);
            }
            var pagedData = query
                .Skip((parameters.Page - 1) * parameters.Length)
                .Take(parameters.Length);
            var reuslt = await _mapper.ProjectTo<TDTO>(pagedData).ToListAsync();
            var totalCount = await query.CountAsync();
            if (parameters.Select?.Length > 0)
            {
                var parameter = Expression.Parameter(typeof(TDTO), "x");
                var bindings = new List<MemberBinding>();

                foreach (var name in parameters.Select)
                {
                    var propertyPath = name.Split('.');
                    Expression propertyExpression = parameter;

                    foreach (var part in propertyPath)
                    {
                        var propertyInfo = propertyExpression.Type.GetProperty(part,
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfo != null)
                            propertyExpression = Expression.PropertyOrField(propertyExpression, part);
                    }

                    if (propertyExpression is MemberExpression memberExpression)
                    {
                        var binding = Expression.Bind(memberExpression.Member, propertyExpression);
                        bindings.Add(binding);
                    }
                }
                if (bindings.Count > 0)
                {
                    var selector = Expression.Lambda<Func<TDTO, TDTO>>(
                        Expression.MemberInit(Expression.New(typeof(TDTO)), bindings),
                        parameter
                    );
                    reuslt = reuslt.AsQueryable().Select(selector.Compile()).ToList();
                }
            }
            return (reuslt, totalCount);
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var set = _dbSet.AsQueryable();
            return await set.Where(predicate).ExecuteDeleteAsync(cancellationToken);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<IEnumerable<TResult>> GetGroupedData<TKey, TResult>(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, TKey>>? groupBy = null,
            Expression<Func<IGrouping<TKey, T>, TResult>>? selector = null,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>>? orderBy = null,
            int? take = null,
            bool trackingChanges = false,
            bool ignoreQueryFilters = false)
        {
            var set = _dbSet.AsQueryable();

            if (ignoreQueryFilters)
            {
                set = set.IgnoreQueryFilters();
            }

            if (predicate != null)
            {
                set = set.Where(predicate);
            }

            set = trackingChanges ? set.AsTracking() : set.AsNoTrackingWithIdentityResolution();

            if (groupBy == null || selector == null)
            {
                throw new ArgumentNullException(nameof(groupBy), "groupBy and selector cannot be null");
            }

            var groupedData = set.GroupBy(groupBy).Select(selector);

            if (orderBy != null)
            {
                groupedData = orderBy(groupedData);
            }

            if (take.HasValue)
            {
                groupedData = groupedData.Take(take.Value);
            }

            return await groupedData.ToListAsync();
        }

        public async Task<bool> AnyExistAsync(
            Expression<Func<T, bool>> predicate,
            bool ignoreQueryFilters = false)
                {
            var set = _dbSet.AsQueryable();

            if (ignoreQueryFilters)
            {
                set = set.IgnoreQueryFilters();
            }

            return await set.AnyAsync(predicate);
        }


    }
}
