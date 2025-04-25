
using Application.Common.Interfaces.Repository;
using AutoMapper;
using DataAccess.Repositories.Base;
using Domain.Common.Entities;

namespace Persistence.Repositories
{
    public class TableOneRepository(ApplicationDbContext dbContext, IMapper mapper) : RepositoryBase<ApplicationDbContext, Table1>(dbContext, mapper), ITableOneRepository
    {

    }
}