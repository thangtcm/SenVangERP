using Application.Common.Interfaces.Repository;
using Application.Common.Interfaces.Services;
using AutoMapper;
using DataAccess.UniOfWork;
using Persistence.Repositories;

namespace Persistence
{
    public class ApplicationUnitOfWork(ApplicationDbContext context, ICurrentUserService user, IMapper mapper) : UnitOfWork<ApplicationDbContext>(context, user)
    {
        private ITableOneRepository _tableOneRepository;
        public ITableOneRepository TableOneRepository
        {
            get
            {
                return _tableOneRepository ??= new TableOneRepository(DbContext, mapper);
            }
        }
    }
}
