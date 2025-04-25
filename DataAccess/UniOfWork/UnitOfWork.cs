using Application.Common.Interfaces.Common;
using Application.Common.Interfaces.Services;
using Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.UniOfWork
{
    public abstract class UnitOfWork<TContext>(TContext context, ICurrentUserService user) : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly Dictionary<Type, Type> _repositories = [];
        private readonly ICurrentUserService _currentUser = user;
        public TContext DbContext => _context;

        public IRepositoryBase<T> GetRepository<T>() where T : class
        {
            var interfaceType = typeof(T);
            if (_repositories == null || !_repositories.ContainsKey(interfaceType))
            {
                throw new KeyNotFoundException(nameof(T));
            }

            var repositoryType = _repositories[interfaceType];
            var repository = Activator.CreateInstance(repositoryType, _context);

            if (repository == null)
            {
                throw new ArgumentException(null, nameof(T));
            }

            return (IRepositoryBase<T>)repository;
        }

        protected void AddRepository<TService, TImplementation>()
            where TImplementation : TService
        {
            AddRepository(typeof(TService), typeof(TImplementation));
        }

        protected void AddRepository(Type serviceType, Type implementationType)
        {
            if (!typeof(IRepositoryBase<>).MakeGenericType(serviceType).IsAssignableFrom(implementationType))
            {
                throw new ArgumentException($"{implementationType.Name} does not implement IRepositoryBase<{serviceType.Name}>.");
            }

            _repositories.Add(serviceType, implementationType);
        }


        public async Task<bool> CompleteAsync() => await SaveAsync() > 0;

        public virtual async Task<int> SaveAsync(string username = "SYSTEM", CancellationToken cancellationToken = default)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            foreach (var entry in _context.ChangeTracker.Entries<ICreatable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = now;
                    entry.Entity.CreatedBy = _currentUser.Name ?? username;
                }
            }

            foreach (var entry in _context.ChangeTracker.Entries<IModifiable>())
            {
                if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                {
                    entry.Entity.ModifiedOn = now;
                    entry.Entity.ModifiedBy = _currentUser.Name ?? username;
                }
            }

            foreach (var entry in _context.ChangeTracker.Entries<IDeletable>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.Entity.DeletedOn = now;
                    entry.Entity.DeletedBy = _currentUser.Name ?? username;
                    entry.Entity.IsDeleted = true;
                    // Đảm bảo rằng dữ liệu không bị xóa vĩnh viễn, chỉ đánh dấu là đã xóa
                    entry.State = EntityState.Modified;
                }
            }
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SoftDeleteRangeAsync<T, TId>(List<TId> ids, string username = "SYSTEM", CancellationToken cancellationToken = default)
            where T : class, IDeletable, IEntity<TId>
        {
            if (ids == null || ids.Count == 0)
                return 0;

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var currentUser = _currentUser.Name ?? username;

            return await _context.Set<T>()
                .Where(e => ids.Contains(e.Id)) 
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(e => e.IsDeleted, true)
                    .SetProperty(e => e.DeletedOn, now)
                    .SetProperty(e => e.DeletedBy, currentUser),
                    cancellationToken);
        }



        #region ITransactionAble Implementation
        public void BeginTransaction()
        => _context.Database.BeginTransaction();
        public void CommitTransaction()
        => _context.Database.CommitTransaction();
        public void RollbackTransaction()
        => _context.Database.RollbackTransaction();
        public void DisposeTransaction()
        {
            if (_context.Database.CurrentTransaction != null)
                _context.Database.CurrentTransaction.Dispose();
        }

        #endregion

        #region IDisposable Implementation
        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories?.Clear();
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        public IQueryable<TView> GetView<TView>() where TView : class
        {
            return _context.Set<TView>().AsNoTracking();
        }

        public async Task<int> ExecuteStoredProcedureAsync(string procedureName, Dictionary<string, string> parameters)
        {
            var commandText = $"EXEC {procedureName} {string.Join(", ", parameters.Select(p => $"@{p.Value}"))}";
            return await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string procedureName, Dictionary<string, string> parameters) where T : class
        {
            var commandText = $"EXEC {procedureName} {string.Join(", ", parameters.Select(p => $"@{p.Value}"))}";
            return await _context.Set<T>().FromSqlRaw(commandText, parameters).ToListAsync();
        }
        #endregion
    }
}
