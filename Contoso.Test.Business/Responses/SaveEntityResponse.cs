using Contoso.Domain;
using LogicBuilder.Attributes;

namespace Contoso.Test.Business.Responses
{
    public class SaveEntityResponse : BaseResponse
    {
        [AlsoKnownAs("SaveEntityResponse_Entity")]
        public EntityModelBase Entity { get; set; }
    }
}
