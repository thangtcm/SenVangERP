using Application.Common.Interfaces.Common;
using Domain.Abstract;
using Domain.Common.Entities;

namespace Application.Common.Interfaces.Repository
{
    public interface ITableOneRepository : IRepositoryBase<Table1>, IScopedService
    {

    }
}