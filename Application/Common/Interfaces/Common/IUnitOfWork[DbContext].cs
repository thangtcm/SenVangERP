using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces.Common
{
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext DbContext { get; }
    }
}
