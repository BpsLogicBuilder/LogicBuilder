using Contoso.Contexts;
using LogicBuilder.EntityFrameworkCore.Crud.DataStores;

namespace Contoso.Stores
{
    public class SchoolStore(SchoolContext context) : StoreBase(context), ISchoolStore
    {
    }
}
