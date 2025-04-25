using Domain.Abstract;

namespace Application.Common.Interfaces.Common
{
    public interface IUnitOfWork : ITransactionAble, IDisposable, IScopedService
    {
        IRepositoryBase<T> GetRepository<T>() where T : class;
        Task<bool> CompleteAsync();
        Task<int> SaveAsync(string username = "SYSTEM", CancellationToken cancellationToken = default);
    }
}
