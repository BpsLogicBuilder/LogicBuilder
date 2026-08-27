using LogicBuilder.Domain.Json;

namespace Contoso.Test.Business.Responses.Json
{
    public class ResponseConverter : JsonTypeConverter<BaseResponse>
    {
        public override string TypePropertyName => "TypeFullName";
    }
}
