using Contoso.Utils;

namespace Contoso.Test.Business.Responses.Json
{
    public class ResponseConverter : JsonTypeConverter<BaseResponse>
    {
        public override string TypePropertyName => "TypeFullName";
    }
}
