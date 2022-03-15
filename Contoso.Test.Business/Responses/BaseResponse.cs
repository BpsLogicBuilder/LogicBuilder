using Contoso.Test.Business.Responses.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Contoso.Test.Business.Responses
{
    [JsonConverter(typeof(ResponseConverter))]
    public abstract class BaseResponse
    {
        public bool Success { get; set; }
        public ICollection<string> ErrorMessages { get; set; }
        public string TypeFullName => this.GetType().AssemblyQualifiedName;
    }
}
