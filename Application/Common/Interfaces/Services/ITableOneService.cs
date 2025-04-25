using Application.Bases;
using Domain.Abstract;
using Domain.Common.Entities;

namespace Application.Common.Interfaces.Services
{
    public interface ITableOneService : IScopedService
    {
        Task<BaseResponse<IEnumerable<Table1>>> GetAll();
    }
}
