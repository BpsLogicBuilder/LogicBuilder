using Contoso.Domain;
using LogicBuilder.Domain;

namespace Contoso.Test.Business.Requests
{
    public class SaveEntityRequest : BaseRequest
    {
        public BaseModel Entity { get; set; }
    }
}
