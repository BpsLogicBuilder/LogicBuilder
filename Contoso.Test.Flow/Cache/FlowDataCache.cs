using Contoso.Test.Business.Requests;
using Contoso.Test.Business.Responses;
using System.Collections.Generic;

namespace Contoso.Test.Flow.Cache
{
    public class FlowDataCache
    {
        public BaseRequest Request { get; set; }
        public BaseResponse Response { get; set; }
        public object[] ObjectArray { get; set; }
        public Dictionary<string, object> Items { get; set; } = new Dictionary<string, object>();
    }
}
