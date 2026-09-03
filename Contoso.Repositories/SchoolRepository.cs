using AutoMapper;
using Contoso.Stores;
using LogicBuilder.EntityFrameworkCore.Repositories;

namespace Contoso.Repositories
{
    public class SchoolRepository(ISchoolStore store, IMapper mapper) : ContextRepositoryBase(store, mapper), ISchoolRepository
    {
    }
}
