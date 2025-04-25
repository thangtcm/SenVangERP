using Application.Bases;
using Application.Common.Interfaces.Services;
using AutoMapper;
using Domain.Common.Entities;

namespace Persistence.Services
{
    public class TableOneService(
        ApplicationUnitOfWork unitOfWork,
        IMapper mapper) : BaseResponseHandler(), ITableOneService
    {
        private readonly IMapper _mapper = mapper;
        private readonly ApplicationUnitOfWork _unitOfWork = unitOfWork;
        public async Task<BaseResponse<IEnumerable<Table1>>> GetAll()
        {
            var request = await _unitOfWork.TableOneRepository.GetData<Table1>();
            return Success(request);
        }
    }
}
