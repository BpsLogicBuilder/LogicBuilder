using Contoso.Domain;

namespace Contoso.Test.Business.Requests
{
    public class SaveEntityRequest : BaseRequest
    {
        public EntityModelBase Entity { get; set; }
    }
}
