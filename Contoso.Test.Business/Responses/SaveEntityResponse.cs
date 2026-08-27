using Contoso.Domain;
using LogicBuilder.Attributes;
using LogicBuilder.Domain;

namespace Contoso.Test.Business.Responses
{
    public class SaveEntityResponse : BaseResponse
    {
        [AlsoKnownAs("SaveEntityResponse_Entity")]
        public BaseModel Entity { get; set; }
    }
}
